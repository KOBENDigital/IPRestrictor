using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Models;
using Koben.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Koben.IPRestrictor.Services.IpDataService
{
	public class WhitelistedIpMapper : IDataModelMapper<WhitelistedIpPoco, WhitelistedIpDto>
	{
		public WhitelistedIpDto Map(WhitelistedIpPoco source)
		{
			if (source == null)
			{
				return null;
			}

			var target = new WhitelistedIpDto(source.Alias, source.FromIp, source.ToIp);
			target.Id = source.Id;

			return target;
		}

		public IEnumerable<WhitelistedIpDto> Map(IEnumerable<WhitelistedIpPoco> sources)
		{
			return sources?.Select(Map) ?? Enumerable.Empty<WhitelistedIpDto>();
		}

		public WhitelistedIpPoco Map(WhitelistedIpDto source)
		{
			if (source == null)
			{
				return null;
			}

			var target = new WhitelistedIpPoco() 
			{ 
				Id = source.Id,
				Alias = source.Alias,
				FromIp = source.FromIp,
				ToIp = source.ToIp
			};

			return target;
		}

		public IEnumerable<WhitelistedIpPoco> Map(IEnumerable<WhitelistedIpDto> sources)
		{
			return sources?.Select(Map) ?? Enumerable.Empty<WhitelistedIpPoco>();
		}
	}
}