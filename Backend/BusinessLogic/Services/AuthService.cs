using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Validators.AppUser;
using BusinessLogic.ViewModels.AppUser;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<UserAuthModel>> LoginAsync(UserLoginModel model)
        {
            var validator = new LoginValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return Result.Fail($"User with email {model.Email} was not found");
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

            if (passwordCorrect is false)
            {
                return Result.Fail("Wrong password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.Succeeded is false)
            {
                return Result.Ok(_mapper.Map<UserAuthModel>(user)).WithError("Unable to log in user");
            }

            return await CreateTokenFor(user);
        }

        public async Task<Result<UserViewModel>> RegisterAsync(UserRegisterModel model)
        {
            var validator = new RegisterValidator();
            var validationResult = validator.Validate(model); 
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));    
            }

            var user = _mapper.Map<AppUser>(model);
            var creationResult = await _userManager.CreateAsync(user, model.Password);
            if (!creationResult.Succeeded)
            {
                return Result.Fail(creationResult.Errors.Select(x => x.Description));   
            }

            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Artist);
            if (!roleResult.Succeeded)
            {
                return Result.Fail(roleResult.Errors.Select(x => x.Description));
            }

            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Role = Roles.Artist;
            return Result.Ok(userViewModel);
        }

        private async Task<Result<UserAuthModel>> CreateTokenFor(AppUser user)
        {
            var tokenResult = await _tokenService.CreateTokenAsync(user);

            if (tokenResult.IsFailed)
            {
                return Result.Ok(_mapper.Map<UserAuthModel>(user)).WithErrors(tokenResult.Errors);
            }

            var userViewModel = _mapper.Map<UserAuthModel>(user);
            userViewModel.Token = tokenResult.Value;
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            userViewModel.Role = role;

            return Result.Ok(userViewModel);
        }
    }
}