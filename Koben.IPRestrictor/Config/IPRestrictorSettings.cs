namespace Koben.IPRestrictor.Config
{
	public class IPRestrictorSettings
	{
		public const string IPRestrictorSection = "IPRestrictor";

		internal const bool StaticEnabled = true;
		internal const string StaticUmbracoPath = "/umbraco";

		public bool Enabled { get; set; } = StaticEnabled;
		public string UmbracoPath { get; set; } = StaticUmbracoPath;
	}
}