using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Storage;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace ItilPaymentFlow.Infrastructure.Storage
{
    public class MinioService : IMinioService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioSettings _settings;

        public MinioService(IMinioClient minioClient, IOptions<MinioSettings> settings)
        {
            _minioClient = minioClient;
            _settings = settings.Value;
        }

        public async Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType)
        {
            // проверка на существование бакета
            bool found = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_settings.BucketName));

            if (!found)
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_settings.BucketName));

            // загрузка файла
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType));

            return $"http{(_settings.WithSSL ? "s" : "")}://{_settings.Endpoint}/{_settings.BucketName}/{fileName}";
        }

        public async Task<List<string>> UploadFileAsync(List<Stream> streams, List<string> fileNames, CancellationToken cancellationToken)
        {
            var result = new List<string>();

            for (int i = 0; i < streams.Count; i++)
            {
                var url = await UploadFileAsync(fileNames[i], streams[i], "application/octet-stream");
                result.Add(url);
            }

            return result;
        }
    }
}