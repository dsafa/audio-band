$VersionToken = '$version$'
$AudioBandVersion = $env:AUDIO_BAND_VERSION
$AssemblyInfoFile = $PSScriptRoot + '/../src/AudioBand/Properties/AssemblyInfo.cs'

(Get-Content -Encoding UTF8 "$AssemblyInfoFile").replace("$VersionToken", "$AudioBandVersion") | Set-Content -Encoding Unicode "$AssemblyInfoFile"