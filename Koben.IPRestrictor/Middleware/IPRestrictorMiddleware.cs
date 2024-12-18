using Koben.IPRestrictor.Config;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetTools;
using System.Net;
using System.Text.RegularExpressions;

namespace Koben.IPRestrictor.Middleware
{
	public class IPRestrictorMiddleware
	{
		private readonly ILogger<IPRestrictorMiddleware> _logger;
		private readonly RequestDelegate _next;
		private readonly IWhiteListedIpDataService _whitelistedIpDataService;
		private readonly IPRestrictorConfigService _iPRestrictorConfigService;

		public IPRestrictorMiddleware
		(
			RequestDelegate next,
			ILogger<IPRestrictorMiddleware> logger,
			IWhiteListedIpDataService whitelistedIpDataService,
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
			try
			{
				if (_iPRestrictorConfigService.Settings.Enabled)
				{
					var umbracoPath = _iPRestrictorConfigService.Settings.UmbracoPath.TrimStart('~');
					var requestedPath = context.Request.Path.ToString();

					if 
					(
						Regex
							.IsMatch
							(
								requestedPath, 
								$@"^{umbracoPath}{_iPRestrictorConfigService.Settings.WhitelistedPathRegex}", 
								RegexOptions.IgnoreCase
							)
						)
					{
						IPAddress hostIpAddress = null;
						try
						{
							var ipString = GetIpAddress(context);
							IPAddress.TryParse(ipString, out hostIpAddress);

							if (hostIpAddress == null)
							{
								_logger.LogError("Ip address failed to parse: {ipString}", ipString);
							}

							var whitelisted = IsWhiteListedIp(hostIpAddress);

							if (!whitelisted)
							{
								if (_iPRestrictorConfigService.Settings.LogWhenBlocking)
								{
									_logger.LogInformation("IP: {hostIpAddress}, IsWhiteListedIp: {whitelisted}", hostIpAddress, whitelisted);
								}

								context.Response.StatusCode = 404;
								context.Response.Headers.Add("iprestrictor-attempted-ip", hostIpAddress?.ToString());
								context.Response.Redirect(_iPRestrictorConfigService.Settings.RedirectUrl);

								return;
							}
							else if (_iPRestrictorConfigService.Settings.LogWhenNotBlocking)
							{
								_logger.LogInformation("IP: {hostIpAddress}, IsWhiteListedIp: {whitelisted}", hostIpAddress, whitelisted);
							};
						}
						catch (Exception ex)
						{
							_logger.LogError(ex, "Ip restrictor threw an error at /umbraco");

							context.Response.StatusCode = 404;
							context.Response.Headers.Add("iprestrictor-attempted-ip", hostIpAddress?.ToString());
							context.Response.Redirect(_iPRestrictorConfigService.Settings.RedirectUrl);

							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ip-Restrictor threw an exception");
			}

			await _next.Invoke(context);
		}

		private string GetIpAddress(HttpContext context)
		{
			if (context.Request.Headers.ContainsKey("CF_Connecting_IP"))
			{
				return context.Request.Headers["CF_Connecting_IP"].ToString();
			}

			if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
			{
				try
				{
					var ipAddresses = context
						.Request
						.Headers["X-Forwarded-For"]
						.ToString()
						.Split(',', StringSplitOptions.RemoveEmptyEntries)
						.ToList();

					var firstIpAddressWithoutAColon = string.Concat(ipAddresses.FirstOrDefault(x => !x.Contains(':'))?.Where(c => !char.IsWhiteSpace(c)) ?? Array.Empty<char>());

					if (_iPRestrictorConfigService.Settings.LogXForwardedFor)
					{
						_logger.LogInformation("X-Forwarded-For value: {0}", context
							.Request
							.Headers["X-Forwarded-For"]
							.ToString());
					}

					return string.IsNullOrWhiteSpace(firstIpAddressWithoutAColon) ? ipAddresses.First().Split(':')[0] : firstIpAddressWithoutAColon;
				}
				catch (Exception ex)
				{
					_logger.LogError
					(
						ex,
						"IP could not be retrieved from X-Forwarded-For header: {header}",
						context
							.Request
							.Headers["X-Forwarded-For"]
							.ToString()
					);

					throw;
				}
			}

			return context.Connection.RemoteIpAddress?.ToString();
		}

		private bool IsWhiteListedIp(IPAddress ip)
		{
			if (ip == null)
			{
				return false;
			}

			var whitelistedIps = GetIPAddressRanges().ToList();

			//We add localhost to the whitelist
			whitelistedIps
				.AddRange
				(
					new[] 
					{ 
						new IPAddressRange(IPAddress.Parse("127.0.0.1")), 
						new IPAddressRange(IPAddress.Parse("0.0.0.1"))
					}
				);

			return whitelistedIps.Any(x => x.Contains(ip.MapToIPv4()));
		}

		/// <summary>
		/// Retrieves configuration data from service transforming it to ranges of addresses
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IPAddressRange> GetIPAddressRanges()
		{
			try
			{
				var data = _whitelistedIpDataService
					.GetAll()
					.Select(x => new IPAddressRange(IPAddress.Parse(x.FromIp), IPAddress.Parse(x.ToIp)));

				return data;
			}
			catch (Exception ex)
			{
				throw new Exception("Error in configuration data.", ex);
			}
		}
	}
}