using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Koben.IpRestrictor.Models;
using Koben.IpRestrictor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetTools;

namespace Koben.IpRestrictor.Middleware
{
	public class IPRestrictorMiddleware
	{
		private readonly ILogger<IPRestrictorMiddleware> _logger;
		private readonly RequestDelegate _next;

		public IPRestrictorMiddleware
		(
			ILogger<IPRestrictorMiddleware> logger,
			RequestDelegate next
		)
		{
			_logger = logger;
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			var umbracoPath = ConfigurationManager.AppSettings["umbracoPath"].TrimStart('~');
			var requestedPath = context.Request.Path.ToString();

			if (requestedPath.StartsWith(umbracoPath) && !requestedPath.ToLower().StartsWith($"{umbracoPath}/api") && !requestedPath.ToLower().StartsWith($"{umbracoPath}/surface"))
			{
				var hostIpAddress = context.Connection.RemoteIpAddress;

				//We check if the IP adddress is a valid address or is not on the whitelist.

				if (!IsWhitelistedIp(hostIpAddress))
				{
					//if we are here is because is a wrong address or isnot whitelisted
					context.Response.StatusCode = 403;
					context.Response.Headers.Add("iprestrictor-attempted-ip", hostIpAddress.ToString());
					context.Response.Redirect("/page-not-found/", true);

					//we cancel request and return a 403.
					//application.CompleteRequest();
				}
			}

			await _next.Invoke(context);
		}

		private bool IsWhitelistedIp(IPAddress ip)
		{
			if (ip == null)
			{
				throw new ArgumentNullException(nameof(ip));
			}


			var whitelistedIps = new List<IPAddressRange>((IEnumerable<IPAddressRange>)ApplicationContext.Current.ApplicationCache.RuntimeCache.GetCacheItem("iprestrictorconfig", () => GetData()));


			//We add localhost to the whitelist
			whitelistedIps.AddRange(new IPAddressRange[] { new IPAddressRange(IPAddress.Parse("127.0.0.1")),
																												new IPAddressRange(IPAddress.Parse("0.0.0.1"))});

			if (whitelistedIps.Any(config => config.Contains(ip.MapToIPv4())))
			{
				_logger.LogInformation("IP " + ip + " is whitelisted");
				return true;
			}
			else
			{
				_logger.LogInformation("IP " + ip + " is NOT whitelisted");
				return false;
			}

		}

		/// <summary>
		/// Retrieves configuration data from service transforming it to ranges of addresses
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IPAddressRange> GetData()
		{
			var service = new IPConfigService();

			try
			{
				var data = service.LoadConfig()
					.Cast<IpConfigData>()
					.Select(ip => IPAddressRange.Parse(ip.FromIp + "-" + ip.ToIp));

				return data;
			}
			catch (Exception ex)
			{
				throw new Exception("Error in configuration data.", ex);
			}
		}
	}

	public static class IPRestrictorMiddlewareExtensions
	{
		public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<IPRestrictorMiddleware>();
		}
	}
}