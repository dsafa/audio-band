param(
    [Parameter(Mandatory = $true)][string]$major,
    [Parameter(Mandatory = $true)][string]$minor,
    [Parameter(Mandatory = $true)][string]$patch
)

$productVersionToken = '<?define ProductVersion = "0.0.0"?>'
$productIdToken = '<?define ProductId = "{CE7B5507-834E-4C1C-85D7-15598EBC52C4}"?>'

$newVersion = "$major.$minor.$patch"
Write-Host "Updating version to $newVersion"

$newGuid = [guid]::NewGuid()
Write-Host "Creating new GUID $newGuid"

$targetFile = $PSScriptRoot + '/../src/AudioBandInstaller/Product.wxs'
$versionReplace = "<?define ProductVersion = '$newVersion'?>"
$guidReplace = "<?define ProductId = '$newGuid'?>"

(Get-Content -Encoding UTF8 "$targetFile").replace("$productVersionToken", "$versionReplace") | Set-Content -Encoding Unicode "$targetFile"
(Get-Content -Encoding UTF8 "$targetFile").replace("$productIdToken", "$guidReplace") | Set-Content -Encoding Unicode "$targetFile"