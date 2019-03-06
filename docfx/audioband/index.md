# Audioband Documentation
Here you will find links to get you started with AudioBand

## Quick Start
### Requirements
- **.Net framework 4.7**
- **Windows 10 anniversary** for High dpi support.

### Installation
There is currently no installer available, however there are prereleases in the [Release](https://github.com/dsafa/audio-band/releases) page that come with a script to install manually.

### Usage
1. If nothing appears on your toolbar after installation, right click on the taskbar and select `Audio Band` from the toolbars submenu.
2. Right click on the Audio Band toolbar and select an audio source (ex. Spotify)
3. [Do any audio source specific setup](audiosources.md)

**IMPORTANT** If nothing happens after selecting `Audio Band` from the toolbars menu or if there are no options in the `Audio Source` menu, some files are being blocked by windows. To fix it, run `unblock.ps1` with powershell. If that doesn't work you can manually fix it by right clicking the files -> properties and clicking unblock. If there are still problems, feel free to post an issue.

![](~/images/hover-over.png)

![](~/images/click-audiosource.png)


## Links
- [AudioSources Setup](audiosources.md)
- [Customization options](customization.md)
- [FAQ](faq.md)
- [Audio source development](~/audiosource-api/index.md)