using ItilPaymentFlow.Api.Middleware;

namespace ItilPaymentFlow.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAdminOnlyForUsers(this IApplicationBuilder app)
        {
            app.UseMiddleware<AdminOnlyMiddleware>();
            return app;
        }
    }
}