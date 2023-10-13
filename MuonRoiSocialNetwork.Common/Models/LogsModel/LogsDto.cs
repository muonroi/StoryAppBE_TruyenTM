using Newtonsoft.Json;
using System;

namespace MuonRoiSocialNetwork.Common.Models.Logs
{
    public class LogsError
    {
        [JsonProperty("fullInfo")]
        public string FullInfo { get; set; } = string.Empty;

        [JsonProperty("messageShort")]
        public string MessageShort { get; set; } = string.Empty;
    }

    public class LogsDto
    {
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("serviceName")]
        public string ServiceName { get; set; } = string.Empty;

        [JsonProperty("apiName")]
        public string ApiName { get; set; } = string.Empty;

        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; } = string.Empty;

        [JsonProperty("durationTime")]
        public long DurationTime { get; set; }

        [JsonProperty("browser")]
        public string Browser { get; set; } = string.Empty;

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("errorMessages")]
        public string ErrorMessages { get; set; } = string.Empty;

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty("request")]
        public string Request { get; set; } = string.Empty;

        [JsonProperty("response")]
        public string Response { get; set; }
    }
}
