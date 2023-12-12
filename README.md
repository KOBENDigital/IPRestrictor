# IPRestrictor
Umbraco package that allows to restrict ip-based access to the backoffice.

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
If the package is configured to use the umbracoDbDSN, the migration will run automatically
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
A new tab titled 'Restrict backoffice access' will be created on the Settings section. 
To add a new ip just use the provided form. You can enter a range of addresses or only one address if you enter the same value in both inputs. Click the 'Add' button to add it to the list.

Don't forget to click 'Save' when you are happy with your whitelist.


<img src="https://raw.githubusercontent.com/KOBENDigital/IPRestrictor/master/docs/settings-screen.png" width="300" alt="Add package" >


The package includes an http module that checks the range of ips entered on the backoffice. The values are saved on the cache so the file is not continuosly read. If the client's ip is not whitelisted it returns a 403. It's up to you to manage that code.

#### Dealing with the 403 code
When the client ip is forbidden, the system will return a 403 error (forbidden). This returns a blank page. You can set up the page that the user will see following this docs:
 - [Letting Umbraco to deal with it](http://letswritecode.net/articles/how-to-setup-custom-error-pages-in-umbraco/)
 - [Letting IIS to deal with it](https://blog.mortenbock.dk/2017/02/03/error-page-setup-in-umbraco/)


### Limitations
Currently only working for IPv4

## Releases
V1.1 - Added CSV parser for better aliases support
