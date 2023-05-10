using Microsoft.AspNetCore.DataProtection;

namespace AuthSystem.Service
{
    public class AuthService
    {
        private readonly IDataProtectionProvider _idp;
        private readonly IHttpContextAccessor _accessor;

        public AuthService(IDataProtectionProvider idp, IHttpContextAccessor accessor)
        {
            _idp = idp;
            _accessor = accessor;
        }

        public void SignIn()
        {
            var protector = _idp.CreateProtector("auth-cookie");
            var httpContext = _accessor.HttpContext;

            if (httpContext != null)
            {
                httpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("user:Daniel Tenkorang")}";
            }

            else
            {
                throw new Exception("No http Context available");
            }

        }
    }
}