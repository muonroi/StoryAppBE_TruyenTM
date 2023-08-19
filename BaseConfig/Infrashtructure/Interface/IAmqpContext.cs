namespace BaseConfig.Infrashtructure.Interface
{
    public interface IAmqpContext
    {
        void ClearHeaders();

        void AddHeaders(IDictionary<string, object> headers);

        string GetHeaderByKey(string headerKey);
    }
}
