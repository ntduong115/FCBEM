using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace FCCore.Middlewares
{
    public class AntiforgeryCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public AntiforgeryCookieMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.PathBase.HasValue)
                {
                    context.Request.PathBase = new PathString(context.Request.PathBase.Value.ToUpperInvariant());
                }
                // Kiểm tra token
                var tokens = _antiforgery.GetAndStoreTokens(context);
            }
            catch (Exception ex)
            {
                // Nếu token không thể giải mã, xóa cookie
                context.Response.Cookies.Delete(".Antiforgery");
            }

            await _next(context);
        }
    }
}
