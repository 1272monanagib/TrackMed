using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.ViewModels.AdminAuthenticationService;
using TrackMed.Shared;

namespace TrackMed.Service.Interfaces
{
    public interface IAdminAuthenticationService
    {
        Task<ServiceResponse<AddAdminResponseViewModel>> AddAdminAsync(AddAdminRequestViewModel addAdminRequestViewModel);
        Task<ServiceResponse<LoginAdminResponseViewModel>> LoginAdminAsync(LoginAdminRequestViewModel loginAdminRequestViewModel);
    }
}
