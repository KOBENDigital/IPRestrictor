using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Koben.IPRestrictor.Controllers
{
	[PluginController("KobenIPRestrictor")]
	public class IpRestrictorController : UmbracoAuthorizedJsonController
	{
		private readonly IWhiteListedIpDataService _whitelistedIpDataService;
		private readonly ILogger<IpRestrictorController> _logger;

		public IpRestrictorController
		(
			IWhiteListedIpDataService whitelistedIpDataService,
			ILogger<IpRestrictorController> logger
		)
		{
			_whitelistedIpDataService = whitelistedIpDataService;
			_logger = logger;
		}

		[HttpPost]
		public IActionResult SaveData([FromBody] IEnumerable<WhiteListedIpDto> data)
		{
			try
			{
				var filtered = data.GroupBy(x => x.Alias).Select(x => x.First());

				var currentIps = _whitelistedIpDataService.GetAll()?.ToList();

				var toDelete = currentIps?.Where(x => filtered.All(y => y.Alias != x.Alias)).ToList();
				if (toDelete?.Any() == true)
				{
					foreach (var ip in toDelete)
					{
						_whitelistedIpDataService.Delete(ip.Id);
					}

					_logger.LogInformation("Deleted the following IP addresses: {0}", string.Join(", ", toDelete.Select(ip => $"{ip.Alias} ({ip.FromIp} > {ip.ToIp})")));
				}

				var toInsert = filtered.Where(x => currentIps != null && IPAddress.TryParse(x.FromIp, out var a) && IPAddress.TryParse(x.ToIp, out var b) && currentIps.All(y => y.Alias != x.Alias)).ToList();

				if (toInsert.Any())
				{
					_whitelistedIpDataService.Insert(toInsert);

					_logger.LogInformation("Added the following IP addresses: {0}", string.Join(", ", string.Join(", ", toInsert.Select(ip => $"{ip.Alias} ({ip.FromIp} > {ip.ToIp})"))));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
			return Ok();
		}

		[HttpGet]
		public IActionResult LoadData()
		{
			try
			{
				var data = _whitelistedIpDataService.GetAll();
				return Ok(data?.Cast<WhiteListedIpDto>());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}
	}
}