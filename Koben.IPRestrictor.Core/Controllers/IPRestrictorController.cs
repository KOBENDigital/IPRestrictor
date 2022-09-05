using Koben.IPRestrictor.Core.Models;
using Koben.IPRestrictor.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace Koben.IPRestrictor.Core.Controllers
{
	[PluginController("KobenIPRestrictor")]
	public class IpRestrictorController : UmbracoAuthorizedJsonController
	{
		private readonly IConfigService _ipConfigService;
		private readonly ILogger<IpRestrictorController> _logger;

		public IpRestrictorController
		(
			IConfigService ipConfigService,
			ILogger<IpRestrictorController> logger
		)
		{
			_ipConfigService = ipConfigService;
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> SaveData([FromBody] IEnumerable<IpConfigData> data)
		{
			try
			{

				await _ipConfigService.SaveConfigAsync(data);
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
				var data = _ipConfigService.LoadConfig();
				return Ok((IEnumerable<IpConfigData>)data.Cast<IpConfigData>());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}
	}
}