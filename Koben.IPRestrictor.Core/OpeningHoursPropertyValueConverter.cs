using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Koben.IPRestrictor.Core
{
    public class OpeningHoursPropertyValueConverter :
        PropertyValueConverterBase
    {
        private static string EditorAlias = "Koben.IPRestrictor";

        public override bool IsConverter(IPublishedPropertyType propertyType)
            => EditorAlias == propertyType.EditorAlias;

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
            => typeof(OpeningHoursModel);

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
            => PropertyCacheLevel.Element;

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
            => OpeningHoursModel.Deserialize((string)source);
    }
}