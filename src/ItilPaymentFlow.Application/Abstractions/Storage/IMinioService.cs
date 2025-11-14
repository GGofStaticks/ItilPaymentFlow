using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Abstractions.Storage
{
    public interface IMinioService
    {
        Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType);

        Task<List<string>> UploadFileAsync(List<Stream> streams, List<string> fileNames, CancellationToken cancellationToken);

    }
}