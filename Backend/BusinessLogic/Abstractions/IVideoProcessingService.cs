using BusinessLogic.ViewModels.VideoProcessing;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface IVideoProcessingService
    {
        Task<Result<string>> ApplyColorFilterToVideoAsync(VideoProcessingModel model);
    }
}