param (

    [Parameter(Mandatory)]
    [string]
    [Alias("v")]
    $version, #version to build

    [Parameter()]
    [string]
    $suffix, # optional suffix to append to version (for pre-releases)

    [Parameter()]
    [string]
    $env = 'release', #build environment to use when packing

    [Parameter()]
    [switch]
    $pushToLocalNugetFeed = $true, #push to a local nuget feed

    [Parameter()]
    [string]
    $localNugetFeed = 'C:\Temp\packages' #local nuget feed location
)

if ($version.IndexOf('-') -ne -1) {
    Write-Host "Version shouldn't contain a - (remember version and suffix are seperate)"
    exit
}

$fullVersion = $version;

if (![string]::IsNullOrWhiteSpace($suffix)) {
    $fullVersion = -join($version, '-', $suffix)
}

$majorFolder = $version.Substring(0, $version.LastIndexOf('.'))

$outFolder = ".\$majorFolder\$version\$fullVersion"

if (![string]::IsNullOrWhiteSpace($suffix)) {
    $suffixFolder = $suffix;
    if ($suffix.IndexOf('.') -ne -1) {
        $suffixFolder = $suffix.substring(0, $suffix.indexOf('.'))
    }
    $outFolder = ".\$majorFolder\$version\$version-$suffixFolder\$fullVersion"
}

"----------------------------------"
Write-Host "Version  :" $fullVersion
Write-Host "Config   :" $env
Write-Host "Folder   :" $outFolder
"----------------------------------"; ""

dotnet restore ..

""; "##### Packaging"; "----------------------------------" ; ""

dotnet pack ..\Koben.IPRestrictor.Core\Koben.IPRestrictor.Core.csproj --no-restore -c $env -o $outFolder /p:ContinuousIntegrationBuild=true,version=$fullVersion

#after installing the package, this version results in the 'App_Plugins/Koben.IPRestrictor' folder being included in the project and a 'Koben.IPRestrictor.Editor.dll' in the bin
#when you uninstall, the 'App_Plugins/Koben.IPRestrictor' folder is removed from the project and disk
dotnet pack ..\Koben.IPRestrictor.Editor\Koben.IPRestrictor.Editor.csproj --no-restore -c $env -o $outFolder /p:ContinuousIntegrationBuild=true,version=$fullVersion

#this results in the 'App_Plugins/Koben.IPRestrictor' folder being copied but included in the project but there is no 'Koben.IPRestrictor.Editor.dll' in the bin
#.\nuget pack "..\Koben.IPRestrictor.Editor\Koben.IPRestrictor.Editor.nuspec" -version $fullVersion -OutputDirectory $outFolder

if ($pushToLocalNugetFeed) {
    #""; "##### Publishing to local nuget feed"; "----------------------------------" ; ""
    .\nuget init $outFolder $localNugetFeed
}

Write-Host "Koben.IPRestrictor Packaged : $fullVersion"

Read-Host -Prompt "Press Enter to exit"