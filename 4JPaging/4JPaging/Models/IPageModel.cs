using System.Collections.Generic;

namespace DV.ProjectMaster.Common.Data.Models
{
    public interface IPageModel<T>
    {
        int TotalItems { get; set; }

        int PageSize { get; set; }

        int CurrentPage { get; set; }

        int NumberOfPages { get; }

        List<T> Items { get; set; }
    }
}
