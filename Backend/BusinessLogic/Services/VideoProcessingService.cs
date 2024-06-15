using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Net;
using BusinessLogic.ViewModels.VideoProcessing;
using FluentResults;
using BusinessLogic.Abstractions;
using BusinessLogic.Validators.VideoProcessing;

namespace BusinessLogic.Services
{
    public class VideoProcessingService : IVideoProcessingService
    {
        private readonly IVideoService _videoService;

        public VideoProcessingService(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<Result<string>> ApplyColorFilterToVideoAsync(VideoProcessingModel model)
        {
            var validator = new VideoProcessingValidator();
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var videoResult = await _videoService.GetVideoAsync("", model.videoId);
            if (videoResult.IsFailed)
            {
                return Result.Fail(videoResult.Errors.Select(e => e.Message));
            }
            var video = videoResult.Value;
            string videoUrl = video.DownloadLink;
            var guid = Guid.NewGuid();
            string localVideoPath = $"input\\{guid}.mp4";
            string outputVideoPath = $"output\\{guid}.mp4";

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(videoUrl, localVideoPath);
            }

            using (var capture = new VideoCapture(localVideoPath))
            {
                if (!capture.IsOpened())
                {
                    return Result.Fail("Cannot open video file.");
                }

                
                int frameWidth = (int) (capture.Get(VideoCaptureProperties.FrameWidth) * model.upscale);
                int frameHeight = (int) (capture.Get(VideoCaptureProperties.FrameHeight) * model.upscale);
                var outSize = new OpenCvSharp.Size(frameWidth, frameHeight);
                double fps = capture.Get(VideoCaptureProperties.Fps);

                using (var writer = new VideoWriter(outputVideoPath, FourCC.H264, fps, outSize, false))
                {
                    if (!writer.IsOpened())
                    {
                        return Result.Fail("Cannot create video writer.");
                    }

                    Mat frame = new Mat();
                    Mat filteredFrame = new Mat();

                    while (true)
                    {
                        capture.Read(frame);

                        if (frame.Empty())
                            break;


                        Cv2.Resize(frame, filteredFrame, outSize);
                        Cv2.CopyTo(filteredFrame, frame);
                        if (model.denoise)
                        {
                            Cv2.FastNlMeansDenoising(frame, filteredFrame);
                            Cv2.CopyTo(filteredFrame, frame);
                        }
                        switch (model.videoFilter)
                        {
                            case Enums.VideoFilters.Gray:
                                Cv2.CvtColor(frame, filteredFrame, ColorConversionCodes.BGR2GRAY);
                                Cv2.CopyTo(filteredFrame, frame);
                                break;

                            case Enums.VideoFilters.Sobel:
                                Cv2.GaussianBlur(frame, filteredFrame, new Size(3, 3), 0);
                                Cv2.CopyTo(filteredFrame, frame);
                                Cv2.CvtColor(frame, filteredFrame, ColorConversionCodes.BGR2GRAY);
                                Cv2.CopyTo(filteredFrame, frame);

                                var gradientX = new Mat();
                                var gradientY = new Mat();
                                Cv2.Sobel(frame, gradientX, MatType.CV_16S, 1, 0);
                                Cv2.Sobel(frame, gradientY, MatType.CV_16S, 0, 1);

                                var absGradientX = new Mat();
                                var absGradientY = new Mat();
                                Cv2.ConvertScaleAbs(gradientX, absGradientX);
                                Cv2.ConvertScaleAbs(gradientY, absGradientY);

                                var gradient = new Mat();
                                Cv2.AddWeighted(absGradientX, 0.5, absGradientY, 0.5, 0, gradient);
                                Cv2.CopyTo(gradient, filteredFrame);
                                Cv2.CopyTo(filteredFrame, frame);
                                break;
                        }
                        

                        // Write the filtered frame to the output video
                        writer.Write(filteredFrame);
                    }

                    frame.Dispose();
                    filteredFrame.Dispose();
                }
            }

            return Result.Ok(outputVideoPath);
        }
    }
}
