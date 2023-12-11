namespace Koben.IPRestrictor.Config
{
	public class IPRestrictorSettings
	{
		public const string IPRestrictorSection = "IPRestrictor";

		public const string StaticDataDbDsnName = "dataDbDSN";

		public bool Enabled { get; set; } = true;
		public string UmbracoPath { get; set; } = "/umbraco";
		public string RedirectUrl { get; set; } = "/error-404";
		public bool LogWhenBlocking { get; set; } = false;
		public bool LogWhenNotBlocking { get; set; } = false;
		public bool LogXForwardedFor { get; set; } = false;
		public string DataDbDSNName { get; set; } = StaticDataDbDsnName;
	}
}