# AudioSource development
This page contains the `AudioBand.AudioSource` api documentation and documentation on developing an audio source for AudioBand.


## Building a new audio source project
1. Create a new class library project
2. [Install the audio source nuget package](https://www.nuget.org/packages/AudioBand.AudioSource/)
3. Create a class to implement `IAudioSource`
```cs
public class AudioSource : IAudioSource
{
    // implementation here
}
```
4. The file `AudioSource.manifest` should be add to the project after installing the nuget package. Edit the file so that the name will matches your asembly file name.
```
AudioSource = "AudioSource.dll"
```

## Deploying your new audio source.
For now, AudioBand reads each sub folder under the `AudioSources` folder. To deploy your new audio source, place your files under a new subfolder in the `AudioSources` directory. **Ensure that your AudioSource.manifest file is also included. You also do not need to copy the AudioBand.AudioSource library files**

The file structure will look like this:
```
Audioband/
|--AudioSources/
   |--NewAudioSource/
      |--Audiosource.dll
      |--AudioSource.manifest
      |--other files
```