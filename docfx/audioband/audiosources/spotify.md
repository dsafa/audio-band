# Spotify

## Setup
1. Login to the [Spotify dashboard](https://developer.spotify.com/dashboard/login) and create a new App. Fill in the details, you can name it whatever you want. This app will be just for AudioBand.
2. Go to the app you created and click `Edit Settings`. Add `http://localhost:80` as a callback url.
    1. You can use a different port for the local webserver (see next step)
3. Right click anywhere in the toolbar > Audio Band Settings > Audio Source Settings and fill in the fields `Spotify Client Id` and `Spotify Client Secret`. You can find them in the same dashboard page for the Spotify app you created.
    1. You can also change the `Callback Port` if needed.
4. Your browser should open asking you to login and allow your spotify app to access your currently playing songs.
    1. If your browser does not open automatically, Right click the toolbar and select Audio Sources > Spotify. It should now open in your default browser. 
6. Sign-in and accept and it should now display song information (make sure spotify is selected as the audio source).

>[!NOTE]
> Currently you need to have the Spotify desktop application for the playback buttons to work properly.

>[!WARNING]
> Proxy settings are currently unavailable.

> Dashboard
![](~/images/spotify-dashboard.png)

> Callback settings
![](~/images/spotify-app-settings-callback.png)
