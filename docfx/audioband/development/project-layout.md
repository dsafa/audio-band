# Project layout

Last commit at this time of writing / update: `f13b2f226991e46704684e060873e03b26441f0e`

Here are the main projects in the solution:
- **AudioBand**: The is the "main" project where audioband lives
- **AudioBand.Logging**: This project contains shared logging facilities
- **AudioBand.AudioSource**: This project contains the audiosource interface
- **AudioSourceHost**: This project contains the assembly used to host an audio source in a separate app domain
- ***AudioSource**: These are the projects for the included audio sources.

# AudioBand project

## Entry point
The entry point for audioband can be found in the `Deskband.cs` file. This is the composition root of the application. It's the equivalent of the main function in a normal winforms or wpf application but instead of calling `Application.Run()`, the root user control is instantiated directly. There is no `main window`. The toolbar is implemented as a WPF usercontrol in `UI/Toolbar/AudioBandToolbar.xaml`

## AudioBandToolbar.xaml
This is the top level user control, equivalent to the main window in wpf or main form in winforms.
### Codebehind
The codebehind listens to size changes and notifies windows to update the deskband size.
### ViewModel
The viewmodel for the toolbar can be found in `ViewModels/AudioBandToolbarViewModel`. It handles the context menu, loading the audio sources, and updating other view models when the audio source changes. When the toolbar is loaded, the `LoadCommand` is invoked, starting the step in initialization: loading the audio sources.

## Audio source loading
Audio source loading is done by the `AudioSource/AudioSourceManager` class. Each audio source is loaded in their own app domain using the `AudioSourceHost` project. The creation and communication with the app domain is done through the `AudioSource/AudioSourceProxy` class. Currently, all audio sources are loaded at the start by the toolbar viewmodel, and there is no file system monitoring for new sources. For each audio source, these steps are performed:
1. Add to the observable collection for the context menu
2. Merge settings.
   1. If there are already saved settings for the audio source, then the settings are applied to the audio source
   2. If there are no previously saved settings, then the default setting values are extracted and saved
   3. Viewmodels for these settings are built and added so they can be manipulated in the settings window ui
3. If the audio source is selected in the settings, then it is activated.

## App settings loading
App settings are loaded and exposed by the `Settings/AppSettings` class. Persistence is done through `Settings/Persistence/PersistSettings`. The serialization format for settings uses the `toml` format. Toml is used because when the project was created, configuration was simple and editing the file in a text editor was the way to change settings. Toml was a good use case for that. Now, the settings have more nesting and more lists, which is less readable with toml however there is now a settings UI so editing the file isn't required. So migration to another serialization format isn't something required for now.

### Settings migrations
As the settings evolve, the serialization format can change so there are settings migrations located in the `Migrations` subfolder. These classes update old configuration files to the latest format. If an older version of the settings is detected, then a lookup is done in `SettingsMigration.cs` to find the appropriate way to transform the settings file. Mapping is done with `AutoMapper`.

## Views and ViewModels
Views and viewmodels are stored together in the `UI` subfolder.

No MVVM frameworks are used, instead there is a `ViewModelBase` implementation and standard `ICommand` implementations.

`ViewModelBase` provides automatic implementations for
- `INotifyPropertyChanged`: Has a `SetProperty` method that automatically calls `INotifyPropertyChanged.PropertyChanged` event if a field value changes. The attribute `AlsoNotify` can also be applied to raise `PropertyChanged` for other properties. Example:
```csharp
[AlsoNotify(nameof(Size))]
public int Width
{
    get => _width;
    set => SetProperty(ref _width, value);
}

public Size Size => new Size(Width, Height);
```
- `INotifyDataError`: Provides a few `RaiseValidationError` methods that raise the `INotifyDataError.ErrorsChanged` event.
- `IResettable`: Custom interface that exposes a method to reset the viewmodel to default values. The base class implements a command to call reset and the method `ResetObject<T>` to reset an object to its default value.
- `BeginEdit`,`EndEdit` and `CancelEdit` commands and methods. The attribute `TrackState` can is used to automatically call those methods.

```csharp
// Automatically calls beginedit method if the value is changed
[TrackState]
public int Width
{
    get => Model.Width; // Using the model as the source for the value
    set => SetProperty(Model, nameof(Model.Width), value); // Set the value in the model
}
```

## Message bus
Under the `Messages` folder there is a simple `IMessageBus` interface. Messages are used sparingly for communicating between the settings window <-> toolbar and between view models.

## Other
### Dpi
Since we are in an interop scenario, there are some issues with automatic dpi handling so it is done manually through the `DpiScaling` behavior located in the `UI/behaviors` folder.

# AudioSourceHost project
The `AudioSourceHost` project is a library to load an audio source and exposes it via `MarshalByRef` objects for cross app domain communication. It uses the `Microsoft Extensibility Framework` to locate and load an `IAudioSource`.