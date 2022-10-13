using System.Collections.Generic;

namespace DV.ProjectMaster.Common.Data.Models
{
    public interface IFilterModel
    {
        IDictionary<string, IEnumerable<string>> Filters { get; set; }
        IDictionary<string, IEnumerable<string>> LessThan { get; set; }
        IDictionary<string, IEnumerable<string>> GreaterThan { get; set; }
    }
}
