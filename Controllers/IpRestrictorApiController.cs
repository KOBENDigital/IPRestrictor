using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Koben.IpRestrictor.Interfaces;
using Koben.IpRestrictor.Models;
using Koben.IpRestrictor.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;
using Microsoft.AspNetCore.Http;

namespace Koben.IpRestrictor.Controllers
{
  [PluginController("KobenIPRestrictor")]
    public class IpRestrictorController : UmbracoApiController
	{
        private IConfigService configService;
        public IpRestrictorController()
        {
            configService = new IPConfigService();
        }

        [HttpPost]
        public async Task<IActionResult> SaveData([FromBody]IEnumerable<IpConfigData> data)
        {
            try
            {

                await configService.SaveConfigAsync(data);
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
            var data = configService.LoadConfig();
            return data.Cast<IpConfigData>();
        }
    }
}
