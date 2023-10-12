using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multipass_hostsfileupdater.Models
{
    internal class MultipassListResult
    {
        [JsonProperty("list")]
        public List<MultipassListItem> List { get; set; }
    }
}
