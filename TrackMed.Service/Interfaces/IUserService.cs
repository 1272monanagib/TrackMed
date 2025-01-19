using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.ViewModels;
using TrackMed.Service.ViewModels.UserService;
using TrackMed.Shared;

namespace TrackMed.Service.Interfaces.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<AddUserResponseViewModel>> AddCustomerAsync (AddUserRequestViewModel addUserRequestViewModel);
        Task<ServiceResponse<LoginUserResponseViewModel>> LoginCustomerAsync (LoginUserRequestViewModel loginUserRequestViewModel);
    }
}
