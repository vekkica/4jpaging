using System;
using System.Collections.Generic;

namespace DV.ProjectMaster.Common.Data.Models
{
    /// <summary>
    /// Page model containing items that match provided conditions, data about
    /// total number of items that given conditions provide, size of page,
    /// number of pages and on which page this data is displayed
    /// </summary>
    /// <typeparam name="T">Type of data that is listed on page</typeparam>
    public class PageModel<T> : IPageModel<T>
    {
        /// <summary>
        /// Total number of pages that given conditions provides
        /// </summary>
        public int NumberOfPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

        /// <summary>
        /// Total number of requested items
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Page number on which this item is contained under given conditions
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// List of requested items
        /// </summary>
        public List<T> Items { get; set; }
    }
}
