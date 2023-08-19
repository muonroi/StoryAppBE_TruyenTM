namespace BaseConfig.BaseDbContext.Common
{
    public class PagingItemsDTO<T>
    {
        public IEnumerable<T>? Items { get; set; }

        public PagingInfoDTO? PagingInfo { get; set; }
    }
}
