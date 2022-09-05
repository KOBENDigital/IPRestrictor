using Koben.IPRestrictor.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koben.IPRestrictor.Core.Interfaces
{
	public interface IConfigService
	{
		Task SaveConfigAsync(IEnumerable<IConfigData> data);
		IEnumerable<IConfigData> LoadConfig();
	}
}
