using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.ViewModels.HospitalAuthenticationService;
using TrackMed.Shared;

namespace TrackMed.Service.Interfaces
{
    public interface IHospitalAuthenticationService
    {
        Task<ServiceResponse<AddHospitalResponseViewModel>> AddHospitalAsync(AddHospitalRequestViewModel addHospitalRequestViewModel);
        Task<ServiceResponse<LoginHospitalResponseViewModel>> LoginHospitalAsync(LoginHospitalRequestViewModel loginHospitalRequestViewModel);
    }
}
