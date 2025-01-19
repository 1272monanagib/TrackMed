using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.Interfaces;
using TrackMed.Service.ViewModels.TokenService;
using TrackMed.Shared;

namespace TrackMed.Service.Implementations
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly UserManager<IdentityUser> _userManager;

        public TokenGenerator(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<GetTokenResponseViewModel> GenerateTokenAsync(IdentityUser identityUser)
        {
            var userRoles = await _userManager.GetRolesAsync(identityUser);

            var Claims = new List<Claim>()
            {
                new(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(type: JwtRegisteredClaimNames.Email, identityUser.Email??""),
                new(type: "Uid" , identityUser.Id)
            };

            Claims.AddRange(collection: userRoles.Select(role => new Claim(type: ClaimTypes.Role, role)));

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: Jwt.Key));

            var signingcredentials = new SigningCredentials(symmetricKey, algorithm: SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: Jwt.Issure,
                audience: Jwt.Audience,
                claims: Claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: signingcredentials
                );

            return new GetTokenResponseViewModel()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ValidTo = jwtSecurityToken.ValidTo,
            };
        }
    }
}
