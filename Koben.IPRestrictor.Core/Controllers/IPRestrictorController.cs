using Koben.IpRestrictor.Core.Models;
using Koben.IpRestrictor.Core.Services;
using Koben.IpRestrictor.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace Koben.IpRestrictor.Core.Controllers
{
	[PluginController("KobenIPRestrictor")]
	public class IpRestrictorController : UmbracoApiController
	{
		private readonly IConfigService _ipConfigService;

		public IpRestrictorController(IConfigService ipConfigService)
		{
			_ipConfigService = ipConfigService;
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
		public IEnumerable<IpConfigData> LoadData()
		{
			var data = _ipConfigService.LoadConfig();
			return data.Cast<IpConfigData>();
		}
	}
}