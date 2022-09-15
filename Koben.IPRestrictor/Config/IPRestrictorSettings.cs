namespace Koben.IPRestrictor.Config
{
	public class IPRestrictorSettings
	{
		public const string IPRestrictorSection = "IPRestrictor";

		internal const bool StaticEnabled = true;
		internal const string StaticUmbracoPath = "/umbraco";
		internal const string StaticRedirectUrl = "/error-404";
		internal const bool StaticLogEnabled = false;

		public bool Enabled { get; set; } = StaticEnabled;
		public string UmbracoPath { get; set; } = StaticUmbracoPath;
		public string RedirectUrl { get; set; } = StaticRedirectUrl;
		public bool LogEnabled { get; set; } = StaticLogEnabled;
	}
}