using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Koben.IpRestrictor.Interfaces;
using Koben.IpRestrictor.Models;
using Koben.IpRestrictor.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Koben.IpRestrictor.Controllers
{
    [PluginController("KobenIPRestrictor")]
    public class IpRestrictorController : UmbracoAuthorizedJsonController
    {
        private IConfigService configService;
        public IpRestrictorController()
        {
            configService = new IPConfigService();
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveData([FromBody]IEnumerable<IpConfigData> data)
        {
            try
            {

                await configService.SaveConfigAsync(data);
            }
            catch
            {
                return InternalServerError();
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
