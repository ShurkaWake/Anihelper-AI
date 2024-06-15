using API.Extensions;
using BusinessLogic.Abstractions;
using BusinessLogic.Validators.Like;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/video")]
    [Authorize]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILikeService _likeService;

        public LikeController(UserManager<AppUser> userManager, ILikeService likeService)
        {
            _userManager = userManager;
            _likeService = likeService;
        }

        [HttpPost("{id:int}/like")]
        public async Task<IActionResult> LikeVideoAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var likeModel = new LikeCreationModel
            {
                userId = user.Id,
                videoId = id
            };
            var result = await _likeService.LikeAsync(likeModel);
            return result.ToNoContent();
        }

        [HttpPost("{id:int}/unlike")]
        public async Task<IActionResult> UnlikeVideoAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var unlikeModel = new LikeDeletionModel
            {
                userId = user.Id,
                videoId = id
            };
            var result = await _likeService.UnlikeAsync(unlikeModel);
            return result.ToNoContent();
        }
    }
}
