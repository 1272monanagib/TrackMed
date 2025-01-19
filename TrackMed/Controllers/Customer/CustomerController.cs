using Microsoft.AspNetCore.Mvc;
using TrackMed.ControllerExtensions;
using TrackMed.Service.Interfaces.UserService;
using TrackMed.Service.ViewModels;
using TrackMed.Service.ViewModels.UserService;

namespace TrackMed.Controllers.Customer
{
    public class CustomerController: BaseController
    {
        private readonly IUserService _userService;

        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Signup")]
        public async Task<ActionResult<AddUserResponseViewModel>> AddCustomerAsync([FromBody]AddUserRequestViewModel addUserRequestViewModel)
        {
            var response = await _userService.AddCustomerAsync(addUserRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginUserResponseViewModel>> LoginCustomerAsync([FromBody]LoginUserRequestViewModel loginUserRequestViewModel)
        {
            var response = await _userService.LoginCustomerAsync(loginUserRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }
    }
}
