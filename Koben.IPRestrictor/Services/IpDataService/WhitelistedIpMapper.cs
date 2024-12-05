using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Models;
using Koben.ObjectMapping.Interfaces;

namespace Koben.IPRestrictor.Services.IpDataService
{
	public class WhiteListedIpMapper : ITwoWayMapper<WhiteListedIpPoco, WhiteListedIpDto>
	{
		public WhiteListedIpDto? Map(WhiteListedIpPoco? source)
		{
			if (source == null)
			{
				return null;
			}

			var target = new WhiteListedIpDto(source.Alias, source.FromIp, source.ToIp);
			target.Id = source.Id;

			return target;
		}

		public IEnumerable<WhiteListedIpDto>? Map(IEnumerable<WhiteListedIpPoco>? sources)
		{
			if (sources == null)
			{
				return null;
			}

			var dtos = new List<WhiteListedIpDto>();

			foreach (var source in sources)
			{
				var dto = Map(source);
				if (dto != null)
				{
					dtos.Add(dto);
				}
			}

			return dtos;
		}

		public WhiteListedIpPoco? Map(WhiteListedIpDto? source)
		{
			if (source == null)
			{
				return null;
			}

			var target = new WhiteListedIpPoco
			{ 
				Id = source.Id,
				Alias = source.Alias,
				FromIp = source.FromIp,
				ToIp = source.ToIp
			};

			return target;
		}

		public IEnumerable<WhiteListedIpPoco>? Map(IEnumerable<WhiteListedIpDto>? sources)
		{
			if (sources == null)
			{
				return null;
			}

			var dtos = new List<WhiteListedIpPoco>();

			foreach (var source in sources)
			{
				var dto = Map(source);
				if (dto != null)
				{
					dtos.Add(dto);
				}
			}

			return dtos;
		}
	}
}