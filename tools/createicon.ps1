$sizes = 16, 32, 48, 128, 256
$image = [System.IO.Path]::GetFullPath($args[0])
if (!$image) {
    Write-Error "Missing image argument"
    exit
}

$tempfiles = @()
foreach ($size in $sizes) {
    $tempfilename = [System.IO.Path]::GetFullPath("temp$size.png")
    $tempfiles += $tempfilename
    magick convert "$image" -resize $size'x'$size -background transparent -extent $size'x'$size $tempfilename
}

magick convert $tempfiles audioband.ico

foreach ($temp in $tempfiles) {
    Remove-Item -Path $temp
}