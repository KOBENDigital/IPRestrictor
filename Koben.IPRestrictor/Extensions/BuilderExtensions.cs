using Koben.IPRestrictor.Config;
using Koben.IPRestrictor.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;

namespace Koben.IPRestrictor.Extensions
{
	public static class BuilderExtensions
	{
		public static IApplicationBuilder UseIPRestrictor(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<IPRestrictorMiddleware>();
		}

		public static IUmbracoBuilder AddIPRestrictorConfigs(this IUmbracoBuilder builder, Action<IPRestrictorSettings> defaultOptions = default)
		{
			if (builder.Services.FirstOrDefault(x => x.ServiceType == typeof(IPRestrictorConfigService)) != null)
			{
				return builder;
			}

			var configSection = builder.Config.GetSection(IPRestrictorSettings.IPRestrictorSection);

			var options = builder.Services.AddOptions<IPRestrictorSettings>()
					.Bind(configSection);

			if (defaultOptions != default)
			{
				options.Configure(defaultOptions);
			}

			options.ValidateDataAnnotations();

			builder.Services.AddSingleton<IPRestrictorConfigService>();

			return builder;
		}
	}
}