using Microsoft.AspNetCore.Mvc;
using TrackMed.ControllerExtensions;
using TrackMed.Service.Interfaces;
using TrackMed.Service.ViewModels.EngineerAuthenticationService;

namespace TrackMed.Controllers.Engineer
{
    public class EngineerAuthenticationController: BaseController
    {
        private readonly IEngineerAuthenticationService _engineerAuthenticationService;

        public EngineerAuthenticationController(IEngineerAuthenticationService engineerAuthenticationService)
        {
            _engineerAuthenticationService = engineerAuthenticationService;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<AddEngineerResponseViewModel>> AddEngineerAsync([FromBody]AddEngineerRequestViewModel addEngineerRequestViewModel)
        {
            var response = await _engineerAuthenticationService.AddEngineerAsync(addEngineerRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }

        public async Task<ActionResult<LoginEngineerResponseViewModel>> LoginEngineerAsync(LoginEngineerRequestViewModel loginEngineerRequestViewModel)
        {
            var response = await _engineerAuthenticationService.LoginEngineerAsync(loginEngineerRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }
    }
}
