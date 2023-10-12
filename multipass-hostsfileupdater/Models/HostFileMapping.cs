using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multipass_hostsfileupdater.Models
{
    internal class HostFileMapping
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("uris")]
        public List<string> Uris { get; set; }
    }
}
