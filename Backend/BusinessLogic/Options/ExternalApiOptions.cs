using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Options
{
    public class ExternalApiOptions
    {
        public const string Section = "ExternalApi";

        public string FileServerKey { get; set; }

        public string ModelsLabKey { get; set; }
    }
}
