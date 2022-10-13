using DV.ProjectMaster.Common.Data.Models;

namespace _4JPaging.Paging
{
    public static class PagingExtensions
    {
        public static IQueryable<TSource> SetPaging<TSource>(this IQueryable<TSource> source, IPagingModel paging, out IPageModel<TSource> pageInfo)
        {
            var tmpQuery = source.SetPagingToQuery(paging, out var pageSize, out var pageIndex);

            var count = source.Count();
            pageSize = pageSize > 0 ? pageSize : count;

            pageInfo = new PageModel<TSource>()
            {
                TotalItems = count,
                CurrentPage = pageIndex + 1,
                PageSize = pageSize
            };

            return tmpQuery;
        }

        private static IQueryable<TSource> SetPagingToQuery<TSource>(this IQueryable<TSource> source, IPagingModel paging, out int pageSize, out int pageIndex)
        {
            if (paging == null)
            {
                pageSize = 0;
                pageIndex = 0;
                return source;
            }

            pageIndex = paging.PageNo < 1 ? 0 : paging.PageNo - 1;
            pageSize = paging.PageSize < 1 ? 10 : paging.PageSize;

            return source.Skip(pageIndex * pageSize).Take(pageSize);
        }
    }
}
