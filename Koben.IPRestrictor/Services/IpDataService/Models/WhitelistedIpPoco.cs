using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Koben.IPRestrictor.Services.IpDataService.Models
{
	[TableName(TableName)]
	[PrimaryKey("Id", AutoIncrement = true)]
	[ExplicitColumns]
	public class WhitelistedIpPoco
	{
		public const string TableName = "WhiteListedIps";

		[PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
		[Column("Id")]
		public int Id { get; set; }
		[Column("UmbracoId")]
		public int UmbracoId { get; set; }
		[Column("Alias")]
		public string Alias { get; set; }
		[Column("FromIp")]
		public string FromIp { get; set; }
		[Column("ToIp")]
		public string ToIp { get; set; }
	}
}