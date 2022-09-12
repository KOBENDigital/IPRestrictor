using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace Koben.IPRestrictor.Controllers
{
	[PluginController("KobenIPRestrictor")]
	public class IpRestrictorController : UmbracoAuthorizedJsonController
	{
		private readonly IWhitelistedIpDataService _whitelistedIpDataService;
		private readonly ILogger<IpRestrictorController> _logger;

		public IpRestrictorController
		(
			IWhitelistedIpDataService whitelistedIpDataService,
			ILogger<IpRestrictorController> logger
		)
		{
			_whitelistedIpDataService = whitelistedIpDataService;
			_logger = logger;
		}

		[HttpPost]
		public IActionResult SaveData([FromBody] IEnumerable<WhitelistedIpDto> data)
		{
			try
			{
				var filtered = data.GroupBy(x => x.Alias).Select(x => x.First());

				var currentIps = _whitelistedIpDataService.GetAll();

				var toDelete = currentIps.Where(x => !filtered.Any(y => y.Alias == x.Alias));
				foreach (var ip in toDelete)
				{
					_whitelistedIpDataService.Delete(ip.Id);
				}

				var toInsert = filtered.Where(x => IPAddress.TryParse(x.FromIp, out var a) && IPAddress.TryParse(x.ToIp, out var b) && !currentIps.Any(y => y.Alias == x.Alias));
				var inserted = _whitelistedIpDataService.Insert(toInsert);
				var c = 0;
			}
			catch
			{
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
				return Ok(data.Cast<WhitelistedIpDto>());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}
	}
}