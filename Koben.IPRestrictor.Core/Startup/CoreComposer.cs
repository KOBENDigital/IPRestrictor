using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Koben.IPRestrictor.Core.Startup
{
	public class CoreComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			var config = builder.Config;

			//koben services
			builder.Services.AddSingleton<Koben.IPRestrictor.Core.Interfaces.IConfigService, Koben.IPRestrictor.Core.Services.IPConfigService>();
		}
	}
}