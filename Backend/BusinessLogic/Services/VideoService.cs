using AutoFilterer.Extensions;
using AutoFilterer.Types;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Options;
using BusinessLogic.Services.Repositories;
using BusinessLogic.Validators.Video;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Category;
using BusinessLogic.ViewModels.ExternalApi;
using BusinessLogic.ViewModels.General;
using BusinessLogic.ViewModels.Video;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class VideoService : IVideoService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly ExternalApiOptions _keyOptions;

        public VideoService(
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IVideoRepository videoRepository,
            IMapper mapper,
            UserManager<AppUser> userManager,
            IHttpClientFactory _factory,
            IOptions<ExternalApiOptions> keyoptions)
        {
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _videoRepository = videoRepository;
            _mapper = mapper;
            _httpClient = _factory.CreateClient();
            _keyOptions = keyoptions.Value;
            _userManager = userManager;
        }

        public async Task<Result<VideoViewModel>> CreateVideoAsync(VideoCreateModel model)
        {
            var validator = new VideoCreateValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var video = _mapper.Map<Video>(model);
            var user = await _userRepository.GetAsync(model.CreatorId);
            if (user == null)
            {
                return Result.Fail("Unable to find creator");
            }

            var category = await _categoryRepository.GetAsync(model.CategoryId);
            if (category == null)
            {
                return Result.Fail("Unable to find category");
            }

            video.Creator = user;
            video.Category = category;

            var generationBody = new VideoGenerationModel
            {
                key = _keyOptions.ModelsLabKey,
                model_id = "zeroscope",
                prompt = model.Prompt,
                negative_prompt = "low quality",
                height = 256,
                width = 256,
                num_frames = model.Seconds * model.Fps,
                fps = model.Fps,
                num_inference_steps = 20,
                guidance_scale = 7,
                upscale_height = model.Height,
                upscale_width = model.Width,
                upscale_num_inference_steps = 20,
                upscale_strength = 0.6f,
                upscale_guidance_scale = 12,
                output_type = "mp4"
            };
            var content = JsonContent.Create(generationBody);
            var response = await _httpClient.PostAsync("https://modelslab.com/api/v6/video/text2video", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<VideoGenerationResponseModel>(responseJson);

            if (responseObject.status == "success" || responseObject.status == "processing")
            {
                video.FetchUrl = $"https://modelslab.com/api/v6/video/fetch/{responseObject.id}";
            }
            else
            {
                return Result.Fail("Unable to create video");
            }

            await _videoRepository.AddAsync(video);

            var confirmationResult = await _videoRepository.ConfirmAsync();
            return confirmationResult > 0
                ? Result.Ok(_mapper.Map<VideoViewModel>(video))
                : Result.Fail("Failed to create video");
        }

        public async Task<Result> UpdateVideoAsync(VideoUpdateModel model)
        {
            var validator = new VideoUpdateValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var video = await _videoRepository.GetAsync(model.Id);
            if (video is null)
            {
                return Result.Fail("Unable to find video");
            }

            var generationBody = new VideoGenerationModel
            {
                key = _keyOptions.ModelsLabKey,
                model_id = "zeroscope",
                prompt = model.Prompt,
                negative_prompt = "low quality",
                height = 256,
                width = 256,
                num_frames = model.Seconds * model.Fps,
                fps = model.Fps,
                num_inference_steps = 20,
                guidance_scale = 7,
                upscale_height = model.Height,
                upscale_width = model.Width,
                upscale_num_inference_steps = 20,
                upscale_strength = 0.6f,
                upscale_guidance_scale = 12,
                output_type = "mp4"
            };
            var content = JsonContent.Create(generationBody);
            var response = await _httpClient.PostAsync("https://modelslab.com/api/v6/video/text2video", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<VideoGenerationResponseModel>(responseJson);

            if (responseObject.status == "success" || responseObject.status == "processing")
            {
                video.FetchUrl = $"https://modelslab.com/api/v6/video/fetch/{responseObject.id}";
                video.FileId = null;
                video.Prompt = model.Prompt;
                video.Seconds = model.Seconds;
                video.Fps = model.Fps;
                video.Height = model.Height;
                video.Width = model.Width;
            }
            else
            {
                return Result.Fail("Unable to create video");
            }

            _videoRepository.Update(video);
            var confirmationResult = await _videoRepository.ConfirmAsync();
            return confirmationResult > 0
                ? Result.Ok()
                : Result.Fail("Failed to update video");
        }

        public async Task<Result<PaginationViewModel<VideoViewModel>>> GetVideosAsync(string userId, PaginationFilterBase filter)
        {
            var videos = (await _videoRepository.GetUserVideos(userId));
            var page = videos.AsQueryable().ApplyFilter(filter);
            var videosModel = _mapper.Map<IEnumerable<VideoViewModel>>(page);
            return new PaginationViewModel<VideoViewModel>
            {
                Data = Result.Ok(videosModel),
                PageCount = (int)Math.Ceiling(videos.Count() / (double)filter.PerPage),
            };
        }

        public async Task<Result<VideoViewModel>> GetVideoAsync(string userId, int videoId)
        {
            var video = await _videoRepository.GetVideoIncludingAll(videoId);
            if (video == null)
            {
                return Result.Fail("Video not found");
            }
            var fetchResult = await FetchAsync(video);
            var videoModel = _mapper.Map<VideoViewModel>(video);
            var like = video.Likes.FirstOrDefault(x => x.User.Id == userId);
            videoModel.IsLiked = like != null;
            videoModel.DownloadLink = $"http://localhost:27785/video/{video.FileId}";
            return videoModel;
        }

        public async Task<Result> DeleteVideoAsync(VideoDeleteModel model)
        {
            var video = await _videoRepository.GetVideoIncludingAll(model.videoId);
            if (video == null)
            {
                return Result.Fail("Video not found");
            }

            var user = await _userRepository.GetAsync(model.userId);
            if (user == null)
            {
                return Result.Fail("User not found");
            }

            var role = await _userManager.GetRolesAsync(user);
            var isAdmin = role.Contains(Roles.Admin);

            if (video.Creator.Id != model.userId && !isAdmin)
            {
                return Result.Fail("No accsess to deleting this video");
            }

            _videoRepository.Remove(video);

            var confirmationResult = await _videoRepository.ConfirmAsync();
            return confirmationResult > 0
                ? Result.Ok()
                : Result.Fail("Failed to delete video");
        }

        private async Task<Result> FetchAsync(Video video)
        {
            if (video.FileId is not null)
            {
                return Result.Ok();
            }

            var fetchBody = new VideoFetchRequestModel
            {
                key = _keyOptions.ModelsLabKey
            };
            var fetchRequestContent = JsonContent.Create(fetchBody);
            var fetchResponse = await _httpClient.PostAsync(video.FetchUrl, fetchRequestContent);
            var fetchResponseJson = await fetchResponse.Content.ReadAsStringAsync();
            var fetchResponseObject = JsonSerializer.Deserialize<VideoFetchResponseModel>(fetchResponseJson);

            if (fetchResponseObject.status != "success")
            {
                return Result.Fail("Video not processed yet");
            }

            var uploadBody = new VideoUploadModel
            {
                url = fetchResponseObject.output.FirstOrDefault(),
                key = _keyOptions.FileServerKey
            };
            var uploadRequestContent = JsonContent.Create(uploadBody);
            var uploadResponse = await _httpClient.PostAsync("http://localhost:27785/video", uploadRequestContent);
            var uploadResponseJson = await uploadResponse.Content.ReadAsStringAsync();
            var uploadResponseObject = JsonSerializer.Deserialize<VideoUploadResponseModel>(uploadResponseJson);

            video.FileId = uploadResponseObject.id;
            _videoRepository.Update(video);
            var confirmationResult = await _videoRepository.ConfirmAsync();
            return confirmationResult > 0
                ? Result.Ok()
                : Result.Fail("Failed to fetch video");
        }
    }
}
