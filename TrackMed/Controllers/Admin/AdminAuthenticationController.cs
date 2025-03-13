using Microsoft.AspNetCore.Mvc;
using TrackMed.ControllerExtensions;
using TrackMed.Service.Interfaces;
using TrackMed.Service.ViewModels.AdminAuthenticationService;

namespace TrackMed.Controllers.Admin
{
    public class AdminAuthenticationController: BaseController
    {
        private readonly IAdminAuthenticationService _adminAuthenticationService;
        public AdminAuthenticationController(IAdminAuthenticationService adminAuthenticationService)
        {
            _adminAuthenticationService = adminAuthenticationService;
        }
        [HttpPost("SignUp")]
        public async Task<ActionResult<AddAdminResponseViewModel>> AddAdminAsync([FromBody]AddAdminRequestViewModel addAdminRequestViewModel)
        {
            var response = await _adminAuthenticationService.AddAdminAsync(addAdminRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginAdminResponseViewModel>> LoginAdminAsync([FromBody]LoginAdminRequestViewModel loginAdminRequestViewModel)
        {
            var response = await _adminAuthenticationService.LoginAdminAsync(loginAdminRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }
    }
}
