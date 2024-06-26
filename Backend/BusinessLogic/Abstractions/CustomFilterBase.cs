﻿using AutoFilterer.Enums;
using AutoFilterer.Types;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace BusinessLogic.Abstractions
{
    public class CustomFilterBase : PaginationFilterBase
    {
        [FromQuery(Name = "page")]
        public override int Page { get => base.Page; set => base.Page = value; }

        [FromQuery(Name = "perPage")]
        public override int PerPage { get => base.PerPage; set => base.PerPage = value; }

        [FromQuery(Name = "sortBy")]
        public override Sorting SortBy { get; set; }
    }
}