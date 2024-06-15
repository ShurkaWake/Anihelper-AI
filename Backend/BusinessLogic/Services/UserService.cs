using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Validators.AppUser;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.General;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            IUserRepository userRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Result> ChangePasswordAsync(string userId, UserChangePasswordModel model)
        {
            var validator = new ChangePasswordValidator();
            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid is false)
            {
                return Result.Fail(validatorResult.Errors.Select(x => x.ErrorMessage));
            }

            var user = await _userRepository.GetAsync(userId);
            if (user is null)
            {
                return Result.Fail("User not found");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded is false)
            {
                return Result.Fail("Old password is wrong");
            }

            await _signInManager.RefreshSignInAsync(user);
            return Result.Ok();
        }

        public async Task<Result<string>> DeleteAsync(string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user is null)
            {
                return Result.Fail("User not found");
            }

            _userRepository.Remove(user);
            return (await _userRepository.ConfirmAsync()) > 0
                ? Result.Ok(user.Id)
                : Result.Fail("Failed to delete User");
        }

        public async Task<PaginationViewModel<UserViewModel>> GetAllUsersAsync(AppUserFilter filter)
        {
            IEnumerable<AppUser> users;
            var pages = 1;
            var perPage = filter.PerPage;
            try
            {
                users = await _userRepository.GetAllAsync(filter);
                filter.Page = 1;
                filter.PerPage = int.MaxValue;
                pages = (await _userRepository.GetAllAsync(filter)).Count();
                pages = (pages / perPage)
                    + (pages % perPage == 0
                        ? 0
                        : 1);
            }
            catch (Exception ex)
            {
                return new PaginationViewModel<UserViewModel>
                {
                    Data = Result.Fail(ex.Message),
                    PageCount = 1,
                };
            }

            var response = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserViewModel>>(users);

            foreach (var user in response)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                var role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();
                user.Role = role;
            }

            return new PaginationViewModel<UserViewModel>
            {
                Data = Result.Ok(response),
                PageCount = pages
            };
        }
    }
}
