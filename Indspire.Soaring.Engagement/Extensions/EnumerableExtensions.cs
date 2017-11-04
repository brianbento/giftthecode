using Indspire.Soaring.Engagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Extensions
{
    public static class EnumerableExtensions
    {
        public static PagedList<TList> ToPagedList<TList>(
            this IEnumerable<TList> list,
            int totalCount,
            int page,
            int pageSize)
        {
            return new PagedList<TList>(list)
            {
                TotalCount = totalCount
            };
        }
    }
}
