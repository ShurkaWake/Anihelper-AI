using API.Extensions;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.AppUser;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] AppUserFilter filter)
        {
            var result = await _userService.GetAllUsersAsync(filter);
            return result.ToObjectResponse();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string id)
        {
            var result = await _userService.DeleteAsync(id);
            return result.ToObjectResponse();
        }

        [HttpPatch("password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePasswordModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userService.ChangePasswordAsync(user.Id, model);
            return result.ToNoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            var response = _mapper.Map<UserViewModel>(user);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            response.Role = role;
            return Result.Ok(response).ToObjectResponse();
        }
    }
}
