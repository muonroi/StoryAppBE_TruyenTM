using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace BaseConfig.Infrashtructure.Filter
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        private readonly AuthContext _authContext;

        public AuthHeaderHandler(ILogger<AuthHeaderHandler> logger, AuthContext authContext)
        {
            _logger = logger;
            _authContext = authContext;
            base.InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string apiKey = Environment.GetEnvironmentVariable("APIKEY");
            if (!string.IsNullOrEmpty(apiKey))
            {
                request.Headers.Add("APIKEY", apiKey);
                request.Headers.Add("SITEKEY", _authContext.SiteKey);
            }

            if (_authContext != null)
            {
                if (!string.IsNullOrEmpty(_authContext.AccessToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authContext.AccessToken.Replace("Bearer ", ""));
                }

                request.Headers.Add("context_guid", _authContext.Guid);
            }

            using (_logger.BeginScope("{@ContextGuid} {@AuthContext}", _authContext.Guid, _authContext))
            {
                _logger.LogInformation($"{request.Method} {request.RequestUri}");
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }
}
