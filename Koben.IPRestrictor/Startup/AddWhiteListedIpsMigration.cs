using Koben.IPRestrictor.Services.IpDataService.Models;
using Koben.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Koben.IPRestrictor.Startup
{
	public class AddWhiteListedIpsMigration : INotificationHandler<UmbracoApplicationStartingNotification>
	{
		private readonly ILogger<AddWhiteListedIpsMigration> _logger;
		private readonly IDatabaseProvider _dbProvider;

		public AddWhiteListedIpsMigration
		(
			ILogger<AddWhiteListedIpsMigration> logger,
			IDatabaseProvider dbProvider
		)
		{
			_logger = logger;
			_dbProvider = dbProvider;
		}

		public void Handle(UmbracoApplicationStartingNotification notification)
		{
			try
			{
				using var db = _dbProvider.GetDatabase();

				var sqlTableExists = $"select case when exists((select * from information_schema.tables where table_name = '{WhiteListedIpPoco.TableName}')) then 1 else 0 end";

				if (db.ExecuteScalar<int>(sqlTableExists) == 0)
				{
					var sqlCreateTable = $"CREATE TABLE [dbo].[{WhiteListedIpPoco.TableName}](" +
						$"[Id] [int] IDENTITY(1,1) NOT NULL, " +
						$"[Alias] [nvarchar](50) NOT NULL, " +
						$"[FromIp] [nvarchar](50) NOT NULL, " +
						$"[ToIp] [nvarchar](50) NOT NULL, " +
						$"[UmbracoId] [int] NULL, " +
						$"CONSTRAINT [PK_{WhiteListedIpPoco.TableName}] PRIMARY KEY CLUSTERED ([Id] ASC )WITH (PAD_INDEX = OFF, " +
						$"STATISTICS_NORECOMPUTE = OFF, " +
						$"IGNORE_DUP_KEY = OFF, " +
						$"ALLOW_ROW_LOCKS = ON, " +
						$"ALLOW_PAGE_LOCKS = ON" +
						$") ON [PRIMARY]) ON [PRIMARY]";

					db.Execute(sqlCreateTable);
				}
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Failed to execute IP-Restrictor migration");
			}
		}
	}
}