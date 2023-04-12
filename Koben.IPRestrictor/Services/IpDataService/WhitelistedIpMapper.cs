using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Models;
using Koben.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Koben.IPRestrictor.Services.IpDataService
{
	public class WhiteListedIpMapper : IDataModelMapper<WhiteListedIpPoco, WhiteListedIpDto>
	{
		public WhiteListedIpDto Map(WhiteListedIpPoco source)
		{
			if (source == null)
			{
				return null;
			}

			var target = new WhiteListedIpDto(source.Alias, source.FromIp, source.ToIp);
			target.Id = source.Id;

			return target;
		}

		public IEnumerable<WhiteListedIpDto> Map(IEnumerable<WhiteListedIpPoco> sources)
		{
			return sources?.Select(Map) ?? Enumerable.Empty<WhiteListedIpDto>();
		}

		public WhiteListedIpPoco Map(WhiteListedIpDto source)
		{
			if (source == null)
			{
				return null;
			}

			var target = new WhiteListedIpPoco() 
			{ 
				Id = source.Id,
				Alias = source.Alias,
				FromIp = source.FromIp,
				ToIp = source.ToIp
			};

			return target;
		}

		public IEnumerable<WhiteListedIpPoco> Map(IEnumerable<WhiteListedIpDto> sources)
		{
			return sources?.Select(Map) ?? Enumerable.Empty<WhiteListedIpPoco>();
		}
	}
}