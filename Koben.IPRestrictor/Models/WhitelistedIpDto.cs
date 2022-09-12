using Newtonsoft.Json;
using System;
using System.Net;

namespace Koben.IPRestrictor.Models
{
	public class WhitelistedIpDto
	{
		public int Id { get; set; }

		[JsonProperty(PropertyName = "alias")]
		public string Alias { get; set; }

		[JsonProperty(PropertyName = "fromIp")]
		public string FromIp { get; set; }

		[JsonProperty(PropertyName = "toIp")]
		public string ToIp { get; set; }

		public WhitelistedIpDto() { }

		public WhitelistedIpDto(string alias, string fromIpStr, string toIpStr)
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

			//remove all spaces
			alias = alias.Replace(" ", "");

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
