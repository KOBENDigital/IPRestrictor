using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koben.IpRestrictor.Interfaces
{
    public interface IConfigService
    {
        Task SaveConfigAsync(IEnumerable<IConfigData> data);
        Task<IEnumerable<IConfigData>> LoadConfigAsync();
    }
}
