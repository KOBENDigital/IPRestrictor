using Koben.IPRestrictor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Koben.IPRestrictor.Startup
{
	public class CoreComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddSingleton<Koben.IPRestrictor.Interfaces.IConfigService, Koben.IPRestrictor.Services.IPConfigService>();
			builder.AddIPRestrictorConfigs();
		}
	}
}