using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewModels.Video
{
    public class VideoUpdateModel
    {
        public int Id { get; set; }

        public string Prompt { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Seconds { get; set; }

        public int Fps { get; set; }
    }
}
