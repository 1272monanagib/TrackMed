using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.Interfaces;
using TrackMed.Service.Interfaces.UserService;
using TrackMed.Service.ViewModels;
using TrackMed.Service.ViewModels.UserService;
using TrackMed.Shared;

namespace TrackMed.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        public UserService(UserManager<IdentityUser> userManager, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<ServiceResponse<AddUserResponseViewModel>> AddCustomerAsync(AddUserRequestViewModel addUserRequestViewModel)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(addUserRequestViewModel.Email);
                if (user is not null)
                {
                    return new ServiceResponse<AddUserResponseViewModel>(data: null, "User with this email already exists");
                }

                var newUser = new IdentityUser()
                {
                    UserName = addUserRequestViewModel.Email,
                    Email = addUserRequestViewModel.Email,
                    PhoneNumber = addUserRequestViewModel.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(newUser);
                if (!result.Succeeded)
                {
                    return new ServiceResponse<AddUserResponseViewModel>(data: null, "Failed to create user");
                }

                var addPassswordResult = await _userManager.AddPasswordAsync(newUser, addUserRequestViewModel.Password);
                if (!addPassswordResult.Succeeded)
                {
                    return new ServiceResponse<AddUserResponseViewModel>(data: null, "Failed to create user");
                }

                var roleResult = await _userManager.AddToRoleAsync(newUser, RolesConst.Customer);
                if (!roleResult.Succeeded)
                {
                    return new ServiceResponse<AddUserResponseViewModel>(data: null, "Failed to create user");
                }

                var generateTokenResult = await _tokenGenerator.GenerateTokenAsync(newUser);
                return new ServiceResponse<AddUserResponseViewModel>(new AddUserResponseViewModel()
                {
                    UserId = newUser.Id,
                    Token = generateTokenResult.Token,
                    ValidTo = generateTokenResult.ValidTo,
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddUserResponseViewModel>(data: null, "Something Went Wrong");
            }
        }

        public async Task<ServiceResponse<LoginUserResponseViewModel>> LoginCustomerAsync(LoginUserRequestViewModel loginUserRequestViewModel)
        {
            try
            {
                var userLogin = await _userManager.FindByEmailAsync(loginUserRequestViewModel.Email);
                if (userLogin is  null)
                {
                    return new ServiceResponse<LoginUserResponseViewModel>(data: null, "User with this email doesn't exist");
                }

                var passwordValid = await _userManager.CheckPasswordAsync(userLogin, loginUserRequestViewModel.Password);
                if (!passwordValid)
                {
                    return new ServiceResponse<LoginUserResponseViewModel>(data: null, "Failed to Login User");
                }

                var isUserInRole = await _userManager.IsInRoleAsync(userLogin, RolesConst.Customer);
                if (!isUserInRole)
                {
                    return new ServiceResponse<LoginUserResponseViewModel>(data: null, "Failed to Login User");
                }

                var tokenResult = await _tokenGenerator.GenerateTokenAsync(userLogin);

                return new ServiceResponse<LoginUserResponseViewModel>(new LoginUserResponseViewModel()
                {
                    UserId = userLogin.Id,
                    Token = tokenResult.Token,
                    ValidTo = tokenResult.ValidTo,
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginUserResponseViewModel>(data: null, "Something Went Wrong");
            }
        }
    }
}
