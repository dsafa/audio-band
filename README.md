[![Build status](https://ci.appveyor.com/api/projects/status/v32xl29r8uucuwj3?svg=true)](https://ci.appveyor.com/project/dsafa/audio-band)

# Audio Band
Audio Band allows you to display song information in the taskbar.

![Demo](./screenshots/demo.gif)

## Features
- Displays song information - album art, artist, title, progress
- Control your music - play/pause, previous/next
- Add support for your music player of choice through plugins

## Installation
There is currently no installer available, however there are prereleases in the [Release](https://github.com/dsafa/audio-band/releases) page that come with a script to install manually.

## Usage
1. Right click on the taskbar and select `Audio Band` from the toolbars submenu.
2. Right click on the Audio Band toolbar and select an audio source (ex. Spotify)

![](./screenshots/hover-over.png)

![](./screenshots/click-audiosource.png)

**IMPORTANT** If nothing happens after selecting `Audio Band` from the toolbars menu or if there are no options in the `Audio Source` menu, some files are being blocked by windows. To fix it, right click the files -> properties and click unblock. If there are still problems, feel free to post an issue.

### Current Supported Audio Sources
- Spotify (**IMPORTANT** due to an [issue](https://github.com/dsafa/audio-band/issues/17), No album art will be displayed. Also the first time it starts, it will only update once spotify is playing a song.)

## Building
This project uses C# 7 features so a compatible compiler is required.

## Contributing
Help is appreciated
- Ask questions, report bugs, suggest features in issues
- Send pull requests

## Screenshots
![Screenshot](./screenshots/screenshot.png)

## License
[LICENSE](https://github.com/dsafa/audio-band/blob/master/LICENSE)

[THIRD PARTY](https://github.com/dsafa/audio-band/blob/master/LICENSE-3RD-PARTY)