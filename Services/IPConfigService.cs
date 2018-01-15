using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Koben.IpRestrictor.Interfaces;
using Koben.IpRestrictor.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace Koben.IpRestrictor.Services
{
    public class IPConfigService : IConfigService
    {
        private readonly string configPath = "~/App_Plugins/IPRestrictor/data/ips.data";
        public async Task SaveConfigAsync(IEnumerable<IConfigData> data)
        {
            var physicalPath = HostingEnvironment.MapPath(configPath);

            try
            {
                using (StreamWriter outputFile = File.AppendText(physicalPath))
                {
                    foreach (IpConfigData item in data)
                    {
                        var line = item.Alias + " " + item.FromIp + " " + item.ToIp;
                        try
                        {
                            await outputFile.WriteLineAsync(line);
                        }
                        catch (Exception ex)
                        {
                            throw new IOException("Error writing config file.", ex);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new IOException("Error writing config file. The 'data' directory doesn't exists.", ex);
            }
            catch (Exception ex)
            {
                throw new IOException("Error writing config file.", ex);
            }

            ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheItem("iprestrictorconfig");
        }


        public IEnumerable<IConfigData> LoadConfig()
        {
            var config = Task.Run(LoadConfigAsync).Result;

            return config;
        }

        /// <summary>
        /// Gets the data in the configuration file
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IConfigData>> LoadConfigAsync()
        {

            var physicalPath = HostingEnvironment.MapPath(configPath);

            if (!File.Exists(physicalPath)) return Enumerable.Empty<IpConfigData>();

            var lines = new List<IpConfigData>();

            try
            {
                using (StreamReader inputFile = new StreamReader(physicalPath))
                {
                    string line;
                    int lineNo = 1;

                    line = await inputFile.ReadLineAsync();
                    while (!String.IsNullOrEmpty(line))
                    {
                        var dataItem = line.Split(' ');
                        if (dataItem.Length != 3) throw new FormatException("There is an error in the ips data file. Line " + lineNo + ".");

                        try
                        {
                            lines.Add(new IpConfigData(alias: dataItem[0],
                                                        fromIpStr: dataItem[1],
                                                        toIpStr: dataItem[2]));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException("Parameters passed to create IpConfigData are invalid", ex);
                        }

                        line = await inputFile.ReadLineAsync();
                    }
                }


            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(IPConfigService), "Error saving configuration", ex);
                throw new Exception("Error saving configuration", ex);
            }

            return lines.ToArray();
        }


    }
}
