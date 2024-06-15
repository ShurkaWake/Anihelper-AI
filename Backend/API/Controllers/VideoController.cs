using API.Extensions;
using API.Requests.Video;
using API.Requests.VideoProcessing;
using AutoFilterer.Types;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.Video;
using BusinessLogic.ViewModels.VideoProcessing;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Controllers
{
    [Route("api/video")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IVideoService _videoService;
        private readonly IVideoProcessingService _videoProcessingService;

        public VideoController(
            IMapper mapper, 
            UserManager<AppUser> userManager, 
            IVideoService videoService,
            IVideoProcessingService videoProcessingService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _videoService = videoService;
            _videoProcessingService = videoProcessingService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVideoAsync([FromBody] VideoCreateRequest request)
        {
            var model = _mapper.Map<VideoCreateModel>(request);
            var user = await _userManager.GetUserAsync(User);
            model.CreatorId = user.Id;
            var result = await _videoService.CreateVideoAsync(model);
            return result.ToObjectResponse();
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetVideoAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = "";
            if (user is not null)
            {
                userId = user.Id;
            }
            var result = await _videoService.GetVideoAsync(userId, id);
            return result.ToObjectResponse();
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetVideosAsync([FromQuery] PaginationFilterBase filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _videoService.GetVideosAsync(user.Id, filter);
            return result.ToObjectResponse();
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateVideoAsync([FromRoute] int id, [FromBody] VideoUpdateRequest request)
        {
            var model = _mapper.Map<VideoUpdateModel>(request);
            model.Id = id;
            var result = await _videoService.UpdateVideoAsync(model);
            return result.ToNoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteVideoAsync([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new VideoDeleteModel
            {
                userId = user.Id,
                videoId = id
            };
            var result = await _videoService.DeleteVideoAsync(model);
            return result.ToNoContent();
        }

        [HttpGet("{id:int}/process")]
        [Authorize]
        public async Task<IActionResult> GetProcessedVideoAsync([FromRoute] int id, [FromQuery] VideoProcessingRequest query)
        {
            var model = _mapper.Map<VideoProcessingModel>(query);
            model.videoId = id;
            var result = await _videoProcessingService.ApplyColorFilterToVideoAsync(model);
            if (result.IsFailed)
            {
                return result.ToObjectResponse();
            }
            var stream = new FileStream(result.Value, FileMode.Open);
            return File(stream, "video/mp4");
        }
    }
}
