using Indspire.Soaring.Engagement.Models;
using Indspire.Soaring.Engagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Extensions
{
    public static class PageListExtensions
    {
        public static Pagination GetPagination<TList>(this PagedList<TList> pagedList)
        {
            var pagination = new Pagination();

            if (pagedList != null)
            {
                pagination = new Pagination()
                {
                    TotalCount = pagedList.TotalCount,

                    PageSize = pagedList.PageSize,

                    Page = pagedList.Page
                };
            }

            return pagination;
        }
    }
}
