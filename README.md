# IPRestrictor
Umbraco package that allows to restrict ip-based access to the backoffice.

## Installation

### Nuget
[![NuGet](https://buildstats.info/nuget/Koben.IpRestrictor)](https://www.nuget.org/packages/Koben.IpRestrictor/)

### Umbraco Package
[![Umb](https://img.shields.io/badge/Package-download-green.svg)](https://our.umbraco.org/projects/backoffice-extensions//)

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
