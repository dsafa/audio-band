# Project layout

Here are the main projects in the solution:
- **AudioBand**: The is the "main" project where audioband lives
- **AudioBand.Logging**: This project contains shared logging facilities
- **AudioBand.AudioSource**: This project contains the audiosource interface
- **AudioSourceHost**: This project contains the assembly used to host an audio source in a separate app domain
- ***AudioSource**: These are the projects for the included audio sources.

# AudioBand project

## Entry point
The entry point for audioband can be found in the `Deskband.cs` file. This is where the main control `MainControl.cs` gets initialized and also the composition root of the application. It's the equivalent of the main function in a normal winforms or wpf application but instead of calling `Application.Run()`, we just instantiate the control directly.

## MainControl.cs
This is the top level user control, equivalent to the main window in wpf or main form in winforms. Note that the file also has 2 other partial classes `MainControl.Bindings.cs` and `MainControl.EventHandlers.cs`. These are the dependencies:
- CSDeskBandOptions: the object that contains deskband settings such as the context menu
- TaskbarInfo: To check the orientation of the taskbar. not currently used
- Track: The track model instance that is shared between the viewmodels that depend on it
- IAppSettings: Contains saving/retrieving the models
- IAudioSourceMananger: Loads the audio sources
- ISettingsWindow: The interface for accessing the settings windows and also the view models for those settings.
- ICustomLabelService: Provides notifications for new/deleting custom labels since they are dynamically generated and not hardcoded in like the other controls.

## Audio source loading
Audio source loading is done by the `AudioSource/AudioSourceManager` class. Each audio source is loaded in their own app domain using the `AudioSourceHost` project. The creation and communication with the app domain is done through the `AudioSource/AudioSourceProxy` class. Audio sources are added to the `IAudioSourceManager.AudioSources` collection and audioband subscribes to the `ObservableCollection.CollectionChanged` event. When new audio sources are added, these steps are performed:
1. Add a new entry to the context menu
2. Merge settings.
   1. If there are already saved settings for the audio source, then the settings are applied to the audio source
   2. If there are no previously saved settings, then the default setting values are extracted and saved.
   3. Viewmodels for these settings are built and added to the `ISettingsWindow.AudioSourceSettings` collection.
3. If the audio source is selected in the settings, then it is activated.