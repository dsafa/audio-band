# Audioband Documentation
Here you will find links to get you started with AudioBand.
See below for installation. See the links on the side for more information.

## Quick Start
### Requirements
- **.Net framework 4.7**
- **Windows 10**

### Installation
There is currently no installer available, however there are prereleases in the [Release](https://github.com/dsafa/audio-band/releases) page that come with a script to install manually.

### Usage
1. If nothing appears on your toolbar after installation, right click on the taskbar and select `Audio Band` from the toolbars submenu. (_See images below_)
2. Right click on the Audio Band toolbar and select an audio source (ex. Spotify)
3. [Do any audio source specific setup](audiosources/index.md)
4. Check out other helpful links in the side

> [!IMPORTANT]
> Previous versions of windows are not supported. Audioband may still work but there is no testing on older versions.

> [!IMPORTANT]
> Windows may block the .zip download. You can right click the zip file > properties > unblock to unblock it. _The unblock option will only be available if it is blocked_

> [!NOTE]
> High DPI is supported on 1703 and above.

### Info
- Logs can be found in the temp folder: `%temp%\AudioBand.log`
- Settings are saved in the app data directory: `%appdata%\AudioBand`

![](~/images/hover-over.png)

![](~/images/click-audiosource.png)


## Links
- [Audio source development](~/audiosource-api/index.md)