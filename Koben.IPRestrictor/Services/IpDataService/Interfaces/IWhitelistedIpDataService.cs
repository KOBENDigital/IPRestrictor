using Koben.IPRestrictor.Models;
using Koben.Persistence.Interfaces;

namespace Koben.IPRestrictor.Services.IpDataService.Interfaces
{
	public interface IWhiteListedIpDataService : IRepository<WhiteListedIpDto, long>
	{
		IEnumerable<WhiteListedIpDto>? GetAll();

		IEnumerable<long> Insert(IEnumerable<WhiteListedIpDto> models);
	}
}