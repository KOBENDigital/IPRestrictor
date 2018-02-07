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

using TinyCsvParser;
using TinyCsvParser.Tokenizer.RFC4180;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace Koben.IpRestrictor.Services
{
    public class IPConfigService : IConfigService
    {
        private readonly string configFolderPath = "~/App_Plugins/IPRestrictor/data";
        private readonly string filename = "ips.data";
        private readonly string filePath;



        public IPConfigService()
        {
            filePath = Path.Combine(HostingEnvironment.MapPath(configFolderPath), filename);
        }

        public async Task SaveConfigAsync(IEnumerable<IConfigData> data)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.


            var lines = new StringBuilder();
            var header = "alias,fromIp,toIp";

            lines.AppendLine(header);

            foreach (IpConfigData item in data)
            {
                var line = $"\"{item.Alias}\",\"{item.FromIp}\",\"{item.ToIp}\"";
                lines.AppendLine(line);
            }

            try
            {
                await Task.Run(() => File.WriteAllText(file.FullName, lines.ToString()));
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new IOException("Error writing config file. The 'data' directory doesn't exists.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error writing config file.", ex);
            }

            ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheItem("iprestrictorconfig");
        }


        /// <summary>
        /// Gets the data in the configuration file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigData> LoadConfig()
        {


            if (!File.Exists(filePath)) return Enumerable.Empty<IpConfigData>();

            var lines = new List<IpConfigData>();

            try
            {
                var options = new Options('"', '\\', ',');
                var tokenizer = new RFC4180Tokenizer(options);
                CsvParserOptions csvParserOptions = new CsvParserOptions(skipHeader: true, tokenizer: tokenizer);
                CsvIpConfigDataMapping csvMapper = new CsvIpConfigDataMapping();
                CsvParser<IpConfigData> csvParser = new CsvParser<IpConfigData>(csvParserOptions, csvMapper);

                var data = csvParser.ReadFromFile(filePath, Encoding.UTF8).ToList();


                foreach (var line in data)
                {

                    if (line.IsValid)
                    {
                        lines.Add(line.Result);
                    }
                    else
                    {
                        LogHelper.Warn(typeof(IPConfigService), "Error parsing IpRestrictor configuration item.");
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
