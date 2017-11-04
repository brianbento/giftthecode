using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Models
{
    public class PagedList<TList> : IEnumerable<TList>
    {
        public int TotalCount { get; set; }

        public IEnumerable<TList> List { get; set; }

        public PagedList(IEnumerable<TList> list)
        {
            this.List = list;
        }

        public IEnumerator<TList> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
