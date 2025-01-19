using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.ViewModels.TokenService;

namespace TrackMed.Service.Interfaces
{
    public interface ITokenGenerator
    {
        Task<GetTokenResponseViewModel> GenerateTokenAsync(IdentityUser identityUser);
    }
}
