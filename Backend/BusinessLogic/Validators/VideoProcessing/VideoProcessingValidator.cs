using BusinessLogic.ViewModels.VideoProcessing;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators.VideoProcessing
{
    public class VideoProcessingValidator : AbstractValidator<VideoProcessingModel>
    {
        public VideoProcessingValidator() 
        {
            RuleFor(x => x.upscale).GreaterThanOrEqualTo(0.1).LessThanOrEqualTo(5);
        }
    }
}
