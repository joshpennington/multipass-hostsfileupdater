using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace multipass_hostsfileupdater.Models
{
    public class MultipassListItem
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("release")]
        public string? Release { get; set; }
        [JsonProperty("state")]
        public string? State { get; set; }
        [JsonProperty("ipv4")]
        public List<string>? Ipv4 { get; set; }
        
        public MultipassListItem()
        {

        }
    }
}