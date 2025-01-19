using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.ViewModels.EngineerAuthenticationService;
using TrackMed.Shared;

namespace TrackMed.Service.Interfaces
{
    public interface IEngineerAuthenticationService
    {
        Task<ServiceResponse<AddEngineerResponseViewModel>> AddEngineerAsync(AddEngineerRequestViewModel addEngineerRequestViewModel);
        Task<ServiceResponse<LoginEngineerResponseViewModel>> LoginEngineerAsync(LoginEngineerRequestViewModel loginEngineerRequestViewModel);
    }
}
