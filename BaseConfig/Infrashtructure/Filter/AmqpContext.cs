using BaseConfig.Infrashtructure.Interface;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BaseConfig.Infrashtructure.Filter
{
    public class AmqpContext : IAmqpContext
    {
        private readonly Dictionary<string, object> _headers;

        public AmqpContext(ILogger<AmqpContext> logger)
        {
            _headers = new Dictionary<string, object>();
        }

        public void ClearHeaders()
        {
            _headers.Clear();
        }

        public void AddHeaders(IDictionary<string, object> headers)
        {
            foreach (KeyValuePair<string, object> header in headers)
            {
                _headers.Add(header.Key, header.Value);
            }
        }

        public string GetHeaderByKey(string headerKey)
        {
            if (_headers.TryGetValue(headerKey, out var value))
            {
                if (value != null)
                {
                    return Encoding.Default.GetString((byte[])value);
                }

                return null;
            }

            return null;
        }
    }
}
