namespace DV.ProjectMaster.Common.Data.Models
{
    public interface IPagingModel
    {
        int PageNo { get; set; }

        int PageSize { get; set; }
    }
}
