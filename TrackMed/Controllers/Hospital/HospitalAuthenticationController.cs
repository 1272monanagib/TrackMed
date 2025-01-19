using Microsoft.AspNetCore.Mvc;
using TrackMed.ControllerExtensions;
using TrackMed.Service.Interfaces;
using TrackMed.Service.ViewModels.HospitalAuthenticationService;

namespace TrackMed.Controllers.Hospital
{
    public class HospitalAuthenticationController: BaseController
    {
        private readonly IHospitalAuthenticationService _hospitalAuthenticationService;

        public HospitalAuthenticationController(IHospitalAuthenticationService hospitalAuthenticationService)
        {
            _hospitalAuthenticationService = hospitalAuthenticationService;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<AddHospitalRequestViewModel>> AddHospitalAsync([FromBody]AddHospitalRequestViewModel addHospitalRequestViewModel)
        {
            var response = await _hospitalAuthenticationService.AddHospitalAsync(addHospitalRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginHospitalResponseViewModel>> LoginHospitalAsync([FromBody]LoginHospitalRequestViewModel loginHospitalRequestViewModel)
        {
            var response = await _hospitalAuthenticationService.LoginHospitalAsync(loginHospitalRequestViewModel);
            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }
    }
}
