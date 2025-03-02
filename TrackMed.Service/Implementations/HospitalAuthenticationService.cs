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
                var hospital = await _userManager.FindByNameAsync(addHospitalRequestViewModel.UserName);
                if (hospital is not null)
                {
                    return new ServiceResponse<AddHospitalResponseViewModel>(data: null, "Hospital with this username already exists");
                }

                var newHospital = new IdentityUser()
                {
                    UserName = addHospitalRequestViewModel.UserName,
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
                var userLogin = await _userManager.FindByNameAsync(loginHospitalRequestViewModel.UserName);
                if (userLogin is null)
                {
                    return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, errrorMessage: "user with username does not exist ");

                }

                var passwordValid = await _userManager.CheckPasswordAsync(userLogin, loginHospitalRequestViewModel.Password);
                if (!passwordValid)
                {
                    return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, errrorMessage: "Failed to login user ");
                }
                var isUserInRole = await _userManager.IsInRoleAsync(userLogin, RolesConst.Customer);


                if (!isUserInRole)
                {
                    return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, errrorMessage: "Failed to login user");
                }

                var tokenResult = await _tokenGenerator.GenerateTokenAsync(userLogin);
                return new ServiceResponse<LoginHospitalResponseViewModel>(new LoginHospitalResponseViewModel()
                {
                    UserId = userLogin.Id,
                    Token = tokenResult.Token,
                    ValidTo = tokenResult.ValidTo
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginHospitalResponseViewModel>(data: null, "Something Went Wrong");
            }
        }
    }
}
