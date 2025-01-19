using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.Interfaces;
using TrackMed.Service.ViewModels;
using TrackMed.Service.ViewModels.HospitalAuthenticationService;
using TrackMed.Service.ViewModels.UserService;
using TrackMed.Shared;

namespace TrackMed.Service.Implementations
{
    public class HospitalAuthenticationService : IHospitalAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        public HospitalAuthenticationService(UserManager<IdentityUser> userManager, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<ServiceResponse<AddHospitalResponseViewModel>> AddHospitalAsync(AddHospitalRequestViewModel addHospitalRequestViewModel)
        {
            try
            {
                var hospital = await _userManager.FindByEmailAsync(addHospitalRequestViewModel.Email);
                if (hospital is not null)
                {
                    return new ServiceResponse<AddHospitalResponseViewModel>(data: null, "Hospital with this email already exists");
                }

                var newHospital = new IdentityUser()
                {
                    UserName = addHospitalRequestViewModel.Email,
                    Email = addHospitalRequestViewModel.Email,
                    PhoneNumber = addHospitalRequestViewModel.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(newHospital);
                if (!result.Succeeded)
                {
                    return new ServiceResponse<AddHospitalResponseViewModel>(data: null, "Failed to create Hospital");
                }

                var addPassswordResult = await _userManager.AddPasswordAsync(newHospital, addHospitalRequestViewModel.Password);
                if (!addPassswordResult.Succeeded)
                {
                    return new ServiceResponse<AddHospitalResponseViewModel>(data: null, "Failed to create Hospital");
                }

                var roleResult = await _userManager.AddToRoleAsync(newHospital, RolesConst.Hospital);
                if (!roleResult.Succeeded)
                {
                    return new ServiceResponse<AddHospitalResponseViewModel>(data: null, "Failed to create Hospital");
                }

                var generateTokenResult = await _tokenGenerator.GenerateTokenAsync(newHospital);
                return new ServiceResponse<AddHospitalResponseViewModel>(new AddHospitalResponseViewModel()
                {
                    UserId = newHospital.Id,
                    Token = generateTokenResult.Token,
                    ValidTo = generateTokenResult.ValidTo,
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddHospitalResponseViewModel>(data: null, "Something Went Wrong");
            }
        }

        public async Task<ServiceResponse<LoginHospitalResponseViewModel>> LoginHospitalAsync(LoginHospitalRequestViewModel loginHospitalRequestViewModel)
        {
            try
            {
                var hospitalLogin = await _userManager.FindByEmailAsync(loginHospitalRequestViewModel.Email);
                if (hospitalLogin is null)
                {
                    return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, "Hospital with this email doesn't exist");
                }

                var passwordValid = await _userManager.CheckPasswordAsync(hospitalLogin, loginHospitalRequestViewModel.Password);
                if (!passwordValid)
                {
                    return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, "Failed to Login Hospital");
                }

                var isHospitalInRole = await _userManager.IsInRoleAsync(hospitalLogin, RolesConst.Hospital);
                if (!isHospitalInRole)
                {
                    return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, "Failed to Login Hospital");
                }

                var tokenResult = await _tokenGenerator.GenerateTokenAsync(hospitalLogin);

                return new ServiceResponse<LoginHospitalResponseViewModel>(new LoginHospitalResponseViewModel()
                {
                    UserId = hospitalLogin.Id,
                    Token = tokenResult.Token,
                    ValidTo = tokenResult.ValidTo,
                });
            }
            catch(Exception ex)
            {
                return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, "Something Went Wrong");
            }
        }
    }
}
