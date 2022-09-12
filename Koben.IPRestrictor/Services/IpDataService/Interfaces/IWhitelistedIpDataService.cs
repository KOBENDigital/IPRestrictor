using Koben.IPRestrictor.Models;
using Koben.Persistence.Interfaces;
using System.Collections.Generic;

namespace Koben.IPRestrictor.Services.IpDataService.Interfaces
{
	public interface IWhitelistedIpDataService : IRepository<WhitelistedIpDto, long>
	{
		IEnumerable<WhitelistedIpDto> GetAll();
	}
}