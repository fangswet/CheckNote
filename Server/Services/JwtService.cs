using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class JwtService
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly HttpContext httpContext;

        public JwtService(UserManager<User> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            //var issuer = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Host}";

            var token = new JwtSecurityToken(
                claims: await GetClaims(user),
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<ICollection<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = await userManager.GetRolesAsync(user);

            // claims.AddRange(await userManager.GetClaimsAsync(user));
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); // not adding claims associated with roles

            return claims;
        }
    }
}
