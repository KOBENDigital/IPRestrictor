using Microsoft.Extensions.Options;

namespace Koben.IPRestrictor.Config
{
	public class IPRestrictorConfigService
	{
		public IPRestrictorSettings Settings => _settingsMonitor.CurrentValue;

		private IOptionsMonitor<IPRestrictorSettings> _settingsMonitor;

		public IPRestrictorConfigService(IOptionsMonitor<IPRestrictorSettings> settingsOptionsMonitor)
		{
			_settingsMonitor = settingsOptionsMonitor;
		}
	}
}