using BusinessLogic.ViewModels.Video;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators.Video
{
    public class VideoCreateValidator : AbstractValidator<VideoCreateModel>
    {
        public VideoCreateValidator() 
        {
            RuleFor(x => x.Tittle).NotEmpty().MaximumLength(48);
            RuleFor(x => x.Prompt).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Height).GreaterThan(127).LessThan(1025);
            RuleFor(x => x.Width).GreaterThan(127).LessThan(1025);
            RuleFor(x => x.Seconds).GreaterThan(1).LessThan(21);
            RuleFor(x => x.Fps).GreaterThan(4).LessThan(26);
        }
    }
}
