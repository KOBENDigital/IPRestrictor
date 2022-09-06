using System.Collections.Generic;
using System.Threading.Tasks;

namespace Koben.IPRestrictor.Interfaces
{
	public interface IConfigService
	{
		Task SaveConfigAsync(IEnumerable<IConfigData> data);
		IEnumerable<IConfigData> LoadConfig();
	}
}