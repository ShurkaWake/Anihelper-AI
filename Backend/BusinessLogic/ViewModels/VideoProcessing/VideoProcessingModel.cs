using BusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewModels.VideoProcessing
{
    public class VideoProcessingModel
    {
        public int videoId { get; set; } 

        public VideoFilters videoFilter { get; set; }

        public bool denoise { get; set; }

        public double upscale { get; set; }
    }
}
