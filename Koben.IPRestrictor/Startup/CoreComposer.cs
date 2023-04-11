using Koben.IPRestrictor.Config;
using Koben.IPRestrictor.Extensions;
using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Koben.IPRestrictor.Services.IpDataService.Models;
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

			builder.Services.AddSingleton<Koben.Persistence.Interfaces.IDataModelMapper<WhitelistedIpPoco, WhitelistedIpDto>, WhitelistedIpMapper>();
			builder.Services.AddSingleton<Koben.Persistence.Interfaces.IDatabaseProvider>
			(x => 
				new Koben.Persistence.NPoco.Persistence.SqlServerDatabaseProvider
				(
					config.GetConnectionString
					(
						config.GetSection(IPRestrictorSettings.IPRestrictorSection)
						.GetValue(nameof(IPRestrictorSettings.DataDbDSNName), IPRestrictorSettings.StaticDataDbDsnName)
					)
				)
			);
			builder.Services.AddSingleton<IWhitelistedIpDataService, WhitelistedIpDataService>();
			builder.AddIPRestrictorConfigs();
		}
	}
}