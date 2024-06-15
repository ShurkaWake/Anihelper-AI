using AutoFilterer.Types;
using BusinessLogic.ViewModels.General;
using BusinessLogic.ViewModels.Video;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IVideoService
    {
        Task<Result<VideoViewModel>> CreateVideoAsync(VideoCreateModel model);

        Task<Result<VideoViewModel>> GetVideoAsync(string userId, int videoId);

        Task<Result<PaginationViewModel<VideoViewModel>>> GetVideosAsync(string userId, PaginationFilterBase filter);

        Task<Result> UpdateVideoAsync(VideoUpdateModel model);

        Task<Result> DeleteVideoAsync(VideoDeleteModel model);
    }
}