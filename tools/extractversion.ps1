param(
    [Parameter(Mandatory = $true)][string]$versionString
)

$pattern = "^v(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(-(?<prerelease>0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))?$"
$version = [regex]::Match($versionString, $pattern)
if ($version.Success) {
    $env:AUDIOBAND_VERSION_MAJOR = $version.Groups["major"]
    $env:AUDIOBAND_VERSION_MINOR = $version.Groups["minor"]
    $env:AUDIOBAND_VERSION_PATCH = $version.Groups["patch"]
    $env:AUDIOBAND_VERSION_PRERELEASE = $version.Groups["prerelease"]
}