using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMed.Service.Interfaces;
using TrackMed.Service.ViewModels;
using TrackMed.Service.ViewModels.EngineerAuthenticationService;
using TrackMed.Service.ViewModels.UserService;
using TrackMed.Shared;

namespace TrackMed.Service.Implementations
{
    public class EngineerAuthenticationService : IEngineerAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        public EngineerAuthenticationService(UserManager<IdentityUser> userManager, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<ServiceResponse<AddEngineerResponseViewModel>> AddEngineerAsync(AddEngineerRequestViewModel addEngineerRequestViewModel)
        {
            try
            {
                var engineer = await _userManager.FindByEmailAsync(addEngineerRequestViewModel.Email);
                if (engineer is not null)
                {
                    return new ServiceResponse<AddEngineerResponseViewModel>(data: null, "Engineer with this email already exists");
                }


                var newEngineer = new IdentityUser()
                {
                    UserName = addEngineerRequestViewModel.Email,
                    Email = addEngineerRequestViewModel.Email,
                    PhoneNumber = addEngineerRequestViewModel.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(newEngineer);
                if (!result.Succeeded)
                {
                    return new ServiceResponse<AddEngineerResponseViewModel>(data: null, "Failed to create Engineer");
                }

                var addPassswordResult = await _userManager.AddPasswordAsync(newEngineer, addEngineerRequestViewModel.Password);
                if (!addPassswordResult.Succeeded)
                {
                    return new ServiceResponse<AddEngineerResponseViewModel>(data: null, "Failed to create Engineer");
                }

                var roleResult = await _userManager.AddToRoleAsync(newEngineer, RolesConst.Engineer);
                if (!roleResult.Succeeded)
                {
                    return new ServiceResponse<AddEngineerResponseViewModel>(data: null, "Failed to create Engineer");
                }

                var generateTokenResult = await _tokenGenerator.GenerateTokenAsync(newEngineer);
                return new ServiceResponse<AddEngineerResponseViewModel>(new AddEngineerResponseViewModel()
                {
                    UserId = newEngineer.Id,
                    Token = generateTokenResult.Token,
                    ValidTo = generateTokenResult.ValidTo,
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddEngineerResponseViewModel>(data: null, "Something Went Wrong");
            }
        }

        public async Task<ServiceResponse<LoginEngineerResponseViewModel>> LoginEngineerAsync(LoginEngineerRequestViewModel loginEngineerRequestViewModel)
        {
            try
            {
                var engineer = await _userManager.FindByEmailAsync(loginEngineerRequestViewModel.Email);
                if (engineer is null)
                {
                    engineer = new IdentityUser()
                    {
                        UserName = loginEngineerRequestViewModel.Email,
                        Email = loginEngineerRequestViewModel.Email
                    };

                    var result = await _userManager.CreateAsync(engineer);
                    if (!result.Succeeded)
                    {
                        return new ServiceResponse<LoginEngineerResponseViewModel>(data: null, "Failed to create Engineer");
                    }
                    var addPasswordResult = await _userManager.AddPasswordAsync(engineer, loginEngineerRequestViewModel.Password);
                    if (!addPasswordResult.Succeeded)
                    {
                        return new ServiceResponse<LoginEngineerResponseViewModel>(data: null, "Failed to Login Engineer");
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(engineer, RolesConst.Engineer);
                    if (!addRoleResult.Succeeded)
                    {
                        return new ServiceResponse<LoginEngineerResponseViewModel>(data: null, "Failed to Login Engineer");
                    }

                }
               
                var tokenResult = await _tokenGenerator.GenerateTokenAsync(engineer);

                return new ServiceResponse<LoginEngineerResponseViewModel>(new LoginEngineerResponseViewModel()
                {
                    UserId = engineer.Id,
                    Token = tokenResult.Token,
                    ValidTo = tokenResult.ValidTo,
                });
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginEngineerResponseViewModel>(data: null, "Something Went Wrong");
            }
        }
    }
}
