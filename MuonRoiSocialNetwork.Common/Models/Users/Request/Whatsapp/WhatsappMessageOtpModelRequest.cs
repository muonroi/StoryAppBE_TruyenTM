using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Users.Request.Whatsapp
{
    public record WhatsappMessageOtpModelRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; } = string.Empty;

        [JsonProperty("recipient_type")]
        public string RecipientType { get; set; } = string.Empty;

        [JsonProperty("to")]
        public string To { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("template")]
        public Template Template { get; set; } = new Template();
    }
    public record Template
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("language")]
        public Language Language { get; set; } = new Language();

        [JsonProperty("components")]
        public List<Component> Components { get; set; } = new List<Component>();
    }
    public record Component
    {
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        [JsonProperty("sub_type")]
        public string SubType { get; set; } = string.Empty;
        [JsonProperty("index")]
        public long Index { get; set; }
    }
    public record Parameter
    {
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }

    public record Language
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;
    }
}
