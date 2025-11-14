using System.Security.Claims;

namespace ItilPaymentFlow.Api.Middleware
{
    public class AdminOnlyMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminOnlyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // проверка маршрута до полей юзеров
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && path.StartsWith("/api/users"))
            {
                var user = context.User;
                var role = user.FindFirstValue(ClaimTypes.Role);

                // если не админ или без акка - 403форбиден
                if (string.IsNullOrEmpty(role) || role != "Admin")
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access denied: Admins only");
                    return;
                }
            }

            // или идем дальше
            await _next(context);
        }
    }
}