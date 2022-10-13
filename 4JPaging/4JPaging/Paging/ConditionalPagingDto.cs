using DV.ProjectMaster.Common.Data.Models;
using System.Collections.Generic;

namespace DV.ProjectMaster.Common.Data.Paging
{
    public class ConditionalPagingDto : IPagingModel, IOrderingModel, IFilterModel
    {
        /// <summary>
        /// Requested page
        /// </summary>
        public int PageNo { get; set; } = 1;

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// List of ordering conditions in combination COLUMN NAME > ORDERING DIRECTION
        /// <example>
        /// OrderBy[LastName]=DESC
        /// OrderBy[FirstName]=ASC
        /// </example>
        /// </summary>
        public IDictionary<string, string> OrderBy { get; set; }

        /// <summary>
        /// List of filters in combination COLUMN NAME > FILTER VALUE
        /// <example>
        /// Filters[status]=new
        /// Filters[status]=toBeAssigned
        /// </example>
        /// </summary>
        public IDictionary<string, IEnumerable<string>> Filters { get; set; }

        /// <summary>
        /// List of filters in combination COLUMN NAME > FILTER VALUE
        /// <example>
        /// LessThan[created]=2020-01-01
        /// </example>
        /// </summary>
        public IDictionary<string, IEnumerable<string>> LessThan { get; set; }

        /// <summary>
        /// List of filters in combination COLUMN NAME > FILTER VALUE
        /// <example>
        /// GreaterThan[created]=2020-01-01
        /// </example>
        /// </summary>
        public IDictionary<string, IEnumerable<string>> GreaterThan { get; set; }
    }
}
