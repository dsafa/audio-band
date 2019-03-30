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

Main steps:
1. Create `MainControl`
2. Load settings
   1. Load saved models
   2. Create viewmodels using the models
   3. Setup databinding for winforms (wpf is done through xaml but winforms is manual)
3. Load Audio sources

## MainControl.cs
This is the top level user control, equivalent to the main window in wpf or main form in winforms. Note that the file also has 2 other partial classes `MainControl.Bindings.cs` and `MainControl.EventHandlers.cs`. These are the dependencies:
- CSDeskBandOptions: the object that contains deskband settings such as the context menu
- TaskbarInfo: To check the orientation of the taskbar. not currently used
- Track: The track model instance that is shared between the viewmodels that depend on it
- IAppSettings: Contains saving/retrieving the models
- IAudioSourceMananger: Loads the audio sources
- ISettingsWindow: The interface for accessing the settings windows and also the view models for those settings.
- ICustomLabelService: Provides notifications for new/deleting custom labels since they are dynamically generated and not hardcoded in like the other controls.

AudioBand uses Winforms controls for the toolbar and Wpf for the settings window.

## Audio source loading
Audio source loading is done by the `AudioSource/AudioSourceManager` class. Each audio source is loaded in their own app domain using the `AudioSourceHost` project. The creation and communication with the app domain is done through the `AudioSource/AudioSourceProxy` class. Audio sources are added to the `IAudioSourceManager.AudioSources` collection and audioband subscribes to the `ObservableCollection.CollectionChanged` event. When new audio sources are added, these steps are performed:
1. Add a new entry to the context menu
2. Merge settings.
   1. If there are already saved settings for the audio source, then the settings are applied to the audio source
   2. If there are no previously saved settings, then the default setting values are extracted and saved.
   3. Viewmodels for these settings are built and added to the `ISettingsWindow.AudioSourceSettings` collection.
3. If the audio source is selected in the settings, then it is activated.

## App settings loading
App settings are loaded by the `Settings/AppSettings` class. It is just simple serialization in the `toml` format. Toml is used because at the start of the project, configuration was simple and the ability to change the values outside of audioband was desired. Toml was a good use case for that. Now, the settings have more nesting and more lists, which is not as readable with toml but it is still workable.

There is also a `Migrations` subfolder that contains code to settings migrations. These classes update old configuration files to the latest format.

## ViewModels
There are two base classes for view models: `ViewModels/ViewModelBase.cs` and `ViewModels/ViewModelBase{TModel}.cs`. Viewmodels participate in databinding for both Winforms and Wpf controls.

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
- Undo support: Implments commands to call `BeginEdit`, `EndEdit`, `CancelEdit` and virtual methods to handle them.

`ViewModelBase<T>` extends `ViewModelBase` by automatically supporting the above features for a model.
- `INotifyPropertyChanged`: Allows the view model to use a model to back its properties by applying the `PropertyChangedBindingAttribute` and using the `SetProperty` method.The `AlsoNotify` property can also be applied. Example:
```csharp
// Bind this property to the 'Width' property of the model.
// When the model's Width property changes, PropertyChanged is invoked for the viewmodel
[PropertyChangeBinding(nameof(Model.Width))]
public int Width
{
    get => Model.Width; // Using the model as the source for the value
    set => SetProperty(nameof(Model.Width), value); // Set the value in the model
}
```
- `IResettable`: When calling reset, the model is automatically reset.
- Undo: Automatic support to undoing changes to the model.

## Winforms controls
Winforms controls are used for the toolbar. The class are under the `Views/Winforms` directory. The controls, including the main control derive from `AudioBandControl` which is a usercontrol that contains support for DPI aware scaling. It exposes two properties `LogicalSize` and `LogicalLocation`. By using those properties, it can automatically scale the `Size` and `Location` properties.