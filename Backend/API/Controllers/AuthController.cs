using API.Extensions;
using API.Requests.Auth;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.ViewModels.AppUser;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(_mapper.Map<UserLoginModel>(request));

            if (result.IsFailed)
            {
                return result.ToObjectResponse();
            }

            return CreatedAtAction("Login", result.Value);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(_mapper.Map<UserRegisterModel>(request));

            if (result.IsFailed)
            {
                return result.ToObjectResponse();
            }

            return CreatedAtAction("Register", result.Value);
        }
    }
}
