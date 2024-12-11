using Koben.IPRestrictor.Config;
using Koben.IPRestrictor.Extensions;
using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Koben.IPRestrictor.Services.IpDataService.Models;
using Koben.ObjectMapping.Interfaces;
using Koben.Persistence.NPoco.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Koben.IPRestrictor.Startup
{
	public class CoreComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			var config = builder.Config;

			builder.Services.AddSingleton<ITwoWayMapper<WhiteListedIpPoco, WhiteListedIpDto>, WhiteListedIpMapper>();
			builder.Services.AddSingleton<IDatabaseProvider>
			(x => 
				new Koben.Persistence.NPoco.Persistence.SqlServerDatabaseProvider
				(
					ConfigurationExtensions.GetConnectionString
					(
						config,
						config.GetSection(IPRestrictorSettings.IPRestrictorSection)
							.GetValue
							(
								nameof(IPRestrictorSettings.DataDbDSNName), 
								IPRestrictorSettings.StaticDataDbDsnName
							)
					)
				)
			);
			builder.Services.AddSingleton<IWhiteListedIpDataService, WhiteListedIpDataService>();
			builder.AddIPRestrictorConfigs();
		}
	}
}