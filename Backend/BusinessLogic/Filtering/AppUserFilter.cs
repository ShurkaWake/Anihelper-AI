using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using BusinessLogic.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Filtering
{
    public class AppUserFilter : CustomFilterBase
    {
        [StringFilterOptions(StringFilterOption.Equals)]
        public string? Id { get; set; }


        [StringFilterOptions(StringFilterOption.Contains)]
        public string? Email { get; set; }


        [StringFilterOptions(StringFilterOption.Contains)]
        public string? FullName { get; set; }


        [StringFilterOptions(StringFilterOption.Contains)]
        public string? Username { get; set; }
    }
}
