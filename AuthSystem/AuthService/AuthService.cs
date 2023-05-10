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
    }
}