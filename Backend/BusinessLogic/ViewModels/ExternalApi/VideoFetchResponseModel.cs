using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewModels.ExternalApi
{
    public class VideoFetchResponseModel
    {
        public string status { get; set; }
        public int id { get; set; }
        public string[] output { get; set; }
        public string[] proxy_links { get; set; }
    }
}
