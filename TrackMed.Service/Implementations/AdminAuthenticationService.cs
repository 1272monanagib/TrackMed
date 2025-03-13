using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.Interfaces;
using TrackMed.Service.ViewModels;
using TrackMed.Service.ViewModels.AdminAuthenticationService;
using TrackMed.Service.ViewModels.UserService;
using TrackMed.Shared;

namespace TrackMed.Service.Implementations
{
    public class AdminAuthenticationService : IAdminAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        public AdminAuthenticationService(UserManager<IdentityUser> userManager , ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<ServiceResponse<AddAdminResponseViewModel>> AddAdminAsync(AddAdminRequestViewModel addAdminRequestViewModel)
        {
            try
            {
                var admin = await _userManager.FindByNameAsync(addAdminRequestViewModel.UserName);
                if (admin is not null)
                {
                    return new ServiceResponse<AddAdminResponseViewModel>(data: null, "Admin with this username already exists");
                }

                var newAdmin = new IdentityUser()
                {
                    UserName = addAdminRequestViewModel.UserName,
                    PhoneNumber = addAdminRequestViewModel.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(newAdmin);
                if (!result.Succeeded)
                {
                    return new ServiceResponse<AddAdminResponseViewModel>(data: null, "Failed to create Admin");
                }

                var addPassswordResult = await _userManager.AddPasswordAsync(newAdmin, addAdminRequestViewModel.Password);
                if (!addPassswordResult.Succeeded)
                {
                    return new ServiceResponse<AddAdminResponseViewModel>(data: null, "Failed to create Admin");
                }

                var roleResult = await _userManager.AddToRoleAsync(newAdmin, RolesConst.Admin);
                if (!roleResult.Succeeded)
                {
                    return new ServiceResponse<AddAdminResponseViewModel>(data: null, "Failed to create Admin");
                }

                var generateTokenResult = await _tokenGenerator.GenerateTokenAsync(newAdmin);
                return new ServiceResponse<AddAdminResponseViewModel>(new AddAdminResponseViewModel()
                {
                    UserId = newAdmin.Id,
                    Token = generateTokenResult.Token,
                    ValidTo = generateTokenResult.ValidTo,
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddAdminResponseViewModel>(data: null, "Something Went Wrong");
            }
        }

        public async Task<ServiceResponse<LoginAdminResponseViewModel>> LoginAdminAsync(LoginAdminRequestViewModel loginAdminRequestViewModel)
        {
            try
            {
                var adminLogin = await _userManager.FindByNameAsync(loginAdminRequestViewModel.UserName);
                if (adminLogin is null)
                {
                    return new ServiceResponse<LoginAdminResponseViewModel>(data: null, errrorMessage: "Admin with username does not exist ");
                }

                var passwordValid = await _userManager.CheckPasswordAsync(adminLogin, loginAdminRequestViewModel.Password);
                if (!passwordValid)
                {
                    return new ServiceResponse<LoginAdminResponseViewModel>(data: null, errrorMessage: "Failed to login Admin ");
                }
                var isAdminInRole = await _userManager.IsInRoleAsync(adminLogin, RolesConst.Admin);


                if (!isAdminInRole)
                {
                    return new ServiceResponse<LoginAdminResponseViewModel>(data: null, errrorMessage: "Failed to login Admin");
                }

                var tokenResult = await _tokenGenerator.GenerateTokenAsync(adminLogin);
                return new ServiceResponse<LoginAdminResponseViewModel>(new LoginAdminResponseViewModel()
                {
                    UserId = adminLogin.Id,
                    Token = tokenResult.Token,
                    ValidTo = tokenResult.ValidTo
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginAdminResponseViewModel>(data: null, "Something Went Wrong");
            }
        }
    }
}
