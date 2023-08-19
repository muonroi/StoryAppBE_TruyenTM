using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Users
{
    public class LocationUserLogin
    {
        [JsonProperty("countryName")]
        public string CountryName { get; set; } = string.Empty;
        [JsonProperty("cityName")]
        public string CityName { get; set; } = string.Empty;
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("timezone")]
        public string TimeZone { get; set; } = string.Empty;
    }
}
