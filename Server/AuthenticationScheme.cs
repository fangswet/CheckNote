using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CheckNote.Server
{
    public class AuthenticationScheme
    {
        public const string Jwt = JwtBearerDefaults.AuthenticationScheme;
        public const string All = Jwt + ",Identity.Application"; // IdentityConstants.ApplicationScheme
    }
}
