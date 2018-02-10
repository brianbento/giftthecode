// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models
{
    public class Pagination
    {
        public int TotalCount { get; set; } = 0;

        public int PageSize { get; set; }

        public int Page { get; set; }

        public string Search { get; set; }

        public string BaseUrl { get; set; } = string.Empty;

        public string QueryStringToken { get; set; } = "?";

        public string QueryStringParam { get; set; } = "page";

        public string QueryStringPageSizeParam { get; set; } = "pageSize";

        public string QueryStringSearchParam { get; set; } = "search";
    }
}
