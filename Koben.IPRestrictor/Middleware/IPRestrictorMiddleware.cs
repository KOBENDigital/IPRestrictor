using Koben.IPRestrictor.Config;
using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Koben.IPRestrictor.Middleware
{
	public class IPRestrictorMiddleware
	{
		private readonly ILogger<IPRestrictorMiddleware> _logger;
		private readonly RequestDelegate _next;
		private readonly IWhitelistedIpDataService _whitelistedIpDataService;
		private readonly IPRestrictorConfigService _iPRestrictorConfigService;

		public IPRestrictorMiddleware
		(
			RequestDelegate next,
			ILogger<IPRestrictorMiddleware> logger,
			IWhitelistedIpDataService whitelistedIpDataService,
			IPRestrictorConfigService iPRestrictorConfigService
		)
		{
			_next = next;
			_logger = logger;
			_whitelistedIpDataService = whitelistedIpDataService;
			_iPRestrictorConfigService = iPRestrictorConfigService;
		}

		public async Task Invoke(HttpContext context)
		{
			var umbracoPath = _iPRestrictorConfigService.Settings.UmbracoPath.TrimStart('~');
			var requestedPath = context.Request.Path.ToString();

			var hostIpAddress = context.Connection.RemoteIpAddress;

			if (requestedPath.StartsWith(umbracoPath) && !requestedPath.ToLower().StartsWith($"{umbracoPath}/api") && !requestedPath.ToLower().StartsWith($"{umbracoPath}/surface"))
			{
				var whitelisted = IsWhitelistedIp(hostIpAddress);

				if (_iPRestrictorConfigService.Settings.LogEnabled)
				{
					_logger.LogInformation($"IP: {hostIpAddress}, IsWhitelistedIp: {whitelisted}");
				}

				if (_iPRestrictorConfigService.Settings.Enabled && !whitelisted)
				{
					//if we are here is because is a wrong address or isnot whitelisted
					context.Response.StatusCode = 403;
					context.Response.Headers.Add("iprestrictor-attempted-ip", hostIpAddress.ToString());
					context.Response.Redirect(_iPRestrictorConfigService.Settings.RedirectUrl, true);

					//we cancel request and return a 403.
					return;
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

			var whitelistedIps = new List<IPAddressRange>
			(
				_whitelistedIpDataService
				.GetAll()
				.Select
				(
					x => new IPAddressRange(IPAddress.Parse(x.FromIp), IPAddress.Parse(x.ToIp))
				)
			);

			//We add localhost to the whitelist
			whitelistedIps.AddRange(new IPAddressRange[] { new IPAddressRange(IPAddress.Parse("127.0.0.1")), new IPAddressRange(IPAddress.Parse("0.0.0.1"))});

			if (whitelistedIps.Any(x => x.Contains(ip.MapToIPv4())))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Retrieves configuration data from service transforming it to ranges of addresses
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IPAddressRange> GetData()
		{
			try
			{
				var data = _whitelistedIpDataService.GetAll()
					.Cast<WhitelistedIpDto>()
					.Select(ip => IPAddressRange.Parse(ip.FromIp + "-" + ip.ToIp));

				return data;
			}
			catch (Exception ex)
			{
				throw new Exception("Error in configuration data.", ex);
			}
		}
	}
}