using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Koben.IpRestrictor.Interfaces;
using Newtonsoft.Json;

namespace Koben.IpRestrictor.Models
{
    public class IpConfigData : IConfigData
    {
        [JsonProperty(PropertyName = "alias")]
        public string Alias { get; set; }

        [JsonProperty(PropertyName = "fromIp")]
        public string FromIp { get; set; }

        [JsonProperty(PropertyName = "toIp")]
        public string ToIp { get; set; }


        public IpConfigData() { }

        public IpConfigData(string alias, string fromIpStr, string toIpStr)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentException("Alias cannot empty", nameof(alias));
            }

            if (string.IsNullOrWhiteSpace(fromIpStr))
            {
                throw new ArgumentException("fromIpStr cannot empty", nameof(fromIpStr));
            }

            if (string.IsNullOrWhiteSpace(toIpStr))
            {
                throw new ArgumentException("toIpStr cannot empty", nameof(toIpStr));
            }

            IPAddress fromIp;
            IPAddress toIp;

            if (!IPAddress.TryParse(fromIpStr, out fromIp))
            {
                throw new ArgumentException("Wrong parameters passed", nameof(fromIpStr));
            }

            if (!IPAddress.TryParse(toIpStr, out toIp))
            {
                throw new ArgumentException("Wrong parameters passed", nameof(toIpStr));
            }

            Alias = alias;
            FromIp = fromIpStr;
            ToIp = toIpStr;

        }

    }
}
