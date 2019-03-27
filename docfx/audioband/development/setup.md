## Setup
1. Install `Visual Studio 2017`. Make sure to install support for `.Net Framework 4.7` development.
2. Clone the repo `git clone git@github.com:dsafa/audio-band.git`
3. Open the solution file under `src/AudioBand.sln` in visual studio.
4. Update nuget packages before building

> [!NOTE]
> Explorer does not unload audioband so you will not be able to build if explorer has loaded it. The `debug configuration` will automatically close explorer to build the audioband project and restart it after the build.

## Running local version
To test the local version of audioband, it needs to be installed as a toolbar. The easiest way is to copy the install script from `tools/install.cmd` to the build folder and run it.

> [!WARNING]
> Installing the development version will overwrite any other installations.

## Debugging
There are 2 ways you can use a debugger on audioband.
- Attach the debugger: In visual studio, open the `attach to process` menu (ctrl + alt + p). Select `explorer.exe` and click attach.
- Insert the statement `System.Diagnostics.Debugger.Launch();` will allow you to attach a debugger at place in the code. This is useful if you need the debugger at the start.