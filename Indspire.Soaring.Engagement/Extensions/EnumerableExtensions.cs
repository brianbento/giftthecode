namespace Indspire.Soaring.Engagement.Extensions
{
    using System.Collections.Generic;
    using Indspire.Soaring.Engagement.Models;

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
                TotalCount = totalCount,

                Page = page,

                PageSize = pageSize                
            };
        }
    }
}
