namespace Koben.IPRestrictor.Config
{
	public class IPRestrictorSettings
	{
		internal const bool StaticEnabled = true;
		internal const string StaticUmbracoPath = "/umbraco";

		public bool Enabled { get; set; } = StaticEnabled;
		public string UmbracoPath { get; set; } = StaticUmbracoPath;
	}
}