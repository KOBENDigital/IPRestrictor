# IPRestrictor
Umbraco package that allows to restric ip-based access to the backoffice.

## Installation

### Nuget
[![NuGet](https://buildstats.info/nuget/Koben.Iconic)](https://www.nuget.org/packages//)

### Umbraco Package
[![Umb](https://img.shields.io/badge/Package-download-green.svg)](https://our.umbraco.org/projects/backoffice-extensions//)

## Usage
A new tab titled 'Restrict backoffice access' will be created on the Settings section. 
To add a new ip just use the provided form. You can enter a range of addresses or only one address if you enter the same value in both inputs. Click the 'Add' button to add it to the list.

Don't forget to click 'Save' when you are happy with your whitelist.


<img src="https://raw.githubusercontent.com/KOBENDigital/IPRestrictor/master/docs/settings-screen.png" width="300" alt="Add package" >




### Limitations
Currently only working for IPv4
