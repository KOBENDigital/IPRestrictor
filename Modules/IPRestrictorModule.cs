using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Koben.IpRestrictor.Models;
using Koben.IpRestrictor.Services;
using NetTools;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace Koben.IpRestrictor.Modules
{
    public class IPRestrictorModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            var umbracoPath = ConfigurationManager.AppSettings["umbracoPath"].TrimStart('~');
            var requestedPath = application.Request.Path;

            if (requestedPath == umbracoPath)
            {
                string hostIp = application.Request.UserHostAddress;

                IPAddress hostIpAddress;

                //We check if the IP adddress is a valid address or is not on the whitelist.

                if (!IPAddress.TryParse(hostIp, out hostIpAddress) ||
                    !IsWhitelistedIp(hostIpAddress))
                {
                    //if we are here is because is a wrong address or isnot whitelisted
                    application.Response.AddHeader("status", "403");
                    application.Response.AddHeader("iprestrictor-attempted-ip", hostIp);

                    //we cancel request and return a 403.
                    application.CompleteRequest();
                }
            }

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
                LogHelper.Info(typeof(IPRestrictorModule), "IP " + ip + " is whitelisted");
                return true;
            }
            else
            {
                LogHelper.Info(typeof(IPRestrictorModule), "IP " + ip + " is NOT whitelisted");
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
}
