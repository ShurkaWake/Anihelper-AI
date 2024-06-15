using BusinessLogic.Enums;

namespace API.Requests.VideoProcessing
{
    public class VideoProcessingRequest
    {
        public VideoFilters? videoFilter { get; set; } = 0;

        public bool? denoise { get; set; } = false;

        public double? upscale { get; set; } = 1;
    }
}
