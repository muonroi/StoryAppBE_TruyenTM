namespace BaseConfig.BaseDbContext.Common
{
    public class PagingInfoDTO
    {
        public int PageSize { get; set; }

        public int Page { get; set; }

        public long TotalItems { get; set; }
    }
}
