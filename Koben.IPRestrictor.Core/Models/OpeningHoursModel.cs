using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace Koben.IPRestrictor.Core
{
    public class OpeningHoursModel : IEnumerable<OpeningTime>
    {
        public IEnumerable<OpeningTime> OpeningTimes { get; set; }

        public IEnumerator<OpeningTime> GetEnumerator()
            => this.OpeningTimes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        public static OpeningHoursModel Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json) || !json.DetectIsJson()) 
                return null;

            var items = JsonConvert.DeserializeObject<IEnumerable<OpeningTime>>(json);
            var model = new OpeningHoursModel()
            {
                OpeningTimes = items
            };

            return model;
        }
    }
}