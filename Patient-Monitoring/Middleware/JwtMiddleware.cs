using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtService _jwt;
        public JwtMiddleware(RequestDelegate next, IJwtService jwt)
        {
            _next = next;
            _jwt = jwt;
        }
        public async Task Invoke(HttpContext context)
        {
            string token = null;
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                token = authHeader.Substring("Bearer ".Length).Trim();
            if (string.IsNullOrEmpty(token) && context.Request.Cookies.TryGetValue("AuthToken", out var cookieToken))
                token = cookieToken;
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var principal = _jwt.ValidateToken(token);
                    context.User = principal;
                }
                catch
                {
                    // ignore invalid tokens -> anonymous user
                }
            }
            await _next(context);
        }
    }
}
