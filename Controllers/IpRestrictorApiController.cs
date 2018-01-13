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
        public async void SaveData([FromBody]IEnumerable<IpConfigData> data)
        {
            if (data == null || data.Count().Equals(0)) return;

            await configService.SaveConfigAsync(data);
        }

        [HttpGet]
        public async Task<IEnumerable<IpConfigData>> LoadData()
        {
            var data = await configService.LoadConfigAsync();
            return data.Cast<IpConfigData>();
        }
    }
}
