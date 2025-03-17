# DEPRECATED
This package has been deprecated as it is legacy and is no longer maintained.
[![No Maintenance Intended](http://unmaintained.tech/badge.svg)](http://unmaintained.tech/)

# IPRestrictor
An Umbraco package that allows restricting access to the Backoffice based on a users IP address.

**Please note:** the package should be installed and configured *after* Umbraco has been installed and database connection strings have been established.

## Installation

### Nuget
[![NuGet](https://buildstats.info/nuget/Koben.IpRestrictor)](https://www.nuget.org/packages/Koben.IpRestrictor/)

### Umbraco Package
[![Umb](https://img.shields.io/badge/Package-download-green.svg)](https://our.umbraco.org/projects/backoffice-extensions//)

## Configuration
### Appsettings
Step 1: Under appsettings, create a section called "IPRestrictor", with:
- bool `Enabled`, which enables and disables the 403 redirects
- string `UmbracoPath`, which will have a default value of `"/umbraco"`
- string `RedirectUrl`, which will have a default value of `"/error-404"`
- bool `LogWhenBlocking`, which will log 403 redirects if enabled. Default value is `false`.
- bool `LogWhenNotBlocking`, which is useful for debugging why users have not been blocked. Default value is `false`.
- bool `LogXForwardedFor`, which is useful for debugging why a particular IP isn't being categorised correctly. Default value is `false`.
- string `DataDbDSNName`, where you will put the key name of the database where whitelisted IPs are stored.
  - Default value of `"dataDbDSN"`
  - Can be changed to `"umbracoDbDSN"` if you only have the CMS database
- string `WhitelistedPathRegex`, which controls which Umbraco paths (EG '/api') are whitelisted for all IPs
	- Default value of `"(?!/[Ss]urface/)(?!/[Aa]pi/)(?!/[Ww]ebservices/)(?!/[Bb]ackoffice/)"`

``` json
"IPRestrictor": {
	"Enabled": true,
	"UmbracoPath": "/umbraco",
	"RedirectUrl": "/error-404",
	"LogWhenBlocking": true,
	"LogWhenNotBlocking": true,
	"LogXForwardedFor": true,
	"DataDbDSNName": "umbracoDbDSN",
	"WhitelistedPathRegex": "(?!/[Ss]urface/)(?!/[Aa]pi/)(?!/[Ww]ebservices/)(?!/[Bb]ackoffice/)"
}
```

### Startup
Step 2: In the web-project `Startup.cs` file:
``` C#
using Koben.IPRestrictor.Extensions;
...
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
	...
	app.UseIPRestrictor();
	...
}
```

## Migration
If the package is configured to use the umbracoDbDSN, the migration will run automatically.
If not, run the following script on your Data-DB to create the "WhiteListedIPs" table:
``` sql
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WhiteListedIPs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[FromIp] [nvarchar](50) NOT NULL,
	[ToIp] [nvarchar](50) NOT NULL,
	[UmbracoId] [int] NULL,
 CONSTRAINT [PK_WhiteListedIPs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

## Usage
A new tab titled 'Restrict Backoffice Access' will be created on the Settings section.  The form allows adding single or ranges of IP addresses.

**Don't forget to click 'Save' when you are happy with your whitelist.**

The package includes middleware that checks the range of IP addresses entered in the Backoffice. If the users IP is not whitelisted the browser returns a 404 and is redirected to the path specified on the `RedirectUrl` app setting property.

### Limitations
Currently only working for IPv4