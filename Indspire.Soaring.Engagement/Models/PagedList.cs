// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models
{
    using System.Collections;
    using System.Collections.Generic;

    public class PagedList<TList> : IEnumerable<TList>
    {
        public PagedList(IEnumerable<TList> list)
        {
            if (list == null)
            {
                this.List = new List<TList>();
            }
            else
            {
                this.List = list;
            }

            this.TotalCount = 0;
            this.Page = 1;
            this.PageSize = 10;
            this.Search = null;
        }

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Search { get; set; }

        public IEnumerable<TList> List { get; set; }

        public IEnumerator<TList> GetEnumerator()
        {
            return this.List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
