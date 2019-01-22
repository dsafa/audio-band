using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Extensions;
using AudioBand.Models;
using AudioBand.ViewModels;
using CSDeskBand.ContextMenu;

namespace AudioBand
{
    partial class MainControl
    {
        private static readonly object _audiosourceListLock = new object();

        #region Winforms event handlers

        private void AlbumArtOnMouseLeave(object o, EventArgs args)
        {
            AlbumArtPopup.Hide(this);
        }

        private void AlbumArtOnMouseHover(object o, EventArgs args)
        {
            AlbumArtPopup.ShowWithoutRequireFocus("Album Art", this, TaskbarInfo);
        }

        private async void PlayPauseButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (_trackModel.IsPlaying)
            {
                await (_currentAudioSource?.PauseTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
            }
            else
            {
                await (_currentAudioSource?.PlayTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
            }
        }

        private async void PreviousButtonOnClick(object sender, EventArgs eventArgs)
        {
            await (_currentAudioSource?.PreviousTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
        }

        private async void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            await (_currentAudioSource?.NextTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
        }

        #endregion

        #region Deskband context menu event handlers

        private void SettingsMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            OpenSettingsWindow();
        }

        private async void AudioSourceMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            await HandleAudioSourceContextMenuItemClick(sender as DeskBandMenuAction).ConfigureAwait(false);
        }

        #endregion

        #region Audio source event handlers

        private void AudioSourceOnTrackProgressChanged(object o, TimeSpan progress)
        {
            BeginInvoke(new Action(() => { _trackModel.TrackProgress = progress; }));
        }

        private void AudioSourceOnTrackPaused(object o, EventArgs args)
        {
            Logger.Debug("State set to paused");

            BeginInvoke(new Action(() => _trackModel.IsPlaying = false));
        }

        private void AudioSourceOnTrackPlaying(object o, EventArgs args)
        {
            Logger.Debug("State set to playing");

            BeginInvoke(new Action(() => _trackModel.IsPlaying = true));
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs trackInfoChangedEventArgs)
        {
            if (trackInfoChangedEventArgs == null)
            {
                Logger.Error("TrackInfoChanged event arg is empty");
                return;
            }

            if (trackInfoChangedEventArgs.TrackName == null)
            {
                trackInfoChangedEventArgs.TrackName = "";
                Logger.Warn("Track name is null");
            }

            if (trackInfoChangedEventArgs.Artist == null)
            {
                trackInfoChangedEventArgs.Artist = "";
                Logger.Warn("Artist is null");
            }

            Logger.Debug($"Track changed - Name: '{trackInfoChangedEventArgs.TrackName}', Artist: '{trackInfoChangedEventArgs.Artist}'");

            BeginInvoke(new Action(() =>
            {
                _trackModel.AlbumArt = trackInfoChangedEventArgs.AlbumArt;
                _trackModel.Artist = trackInfoChangedEventArgs.Artist;
                _trackModel.TrackName = trackInfoChangedEventArgs.TrackName;
                _trackModel.TrackLength = trackInfoChangedEventArgs.TrackLength;
                _trackModel.AlbumName = trackInfoChangedEventArgs.Album;
            }));
        }

        #endregion

        private void Saved(object o, EventArgs eventArgs)
        {
            _settingsWindowVm.EndEdit();
            _appSettings.Save();
        }

        private void Canceled(object sender, EventArgs e)
        {
            _settingsWindowVm.CancelEdit();
        }

        private async void AudioSourcesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var audioSource in e.NewItems.Cast<IInternalAudioSource>())
                {
                    await AddNewAudioSource(audioSource);
                }
            }
            else
            {
                Logger.Warn($"Action {e.Action} not supported");
            }
        }

        private async Task AddNewAudioSource(IInternalAudioSource audioSource)
        {
            var menuItem = new DeskBandMenuAction(audioSource.Name);
            menuItem.Clicked += AudioSourceMenuItemOnClicked;

            lock (_audiosourceListLock)
            {
                _audioSourceContextMenuItems.Add(menuItem);
                RefreshContextMenu();
            }

            var settings = audioSource.Settings;
            if (settings.Count > 0)
            {
                // check if the settings were saved previously and reuse them
                var matchingSetting = _audioSourceSettingsModel.FirstOrDefault(s => s.AudioSourceName == audioSource.Name);
                if (matchingSetting != null)
                {
                    var viewModel = new AudioSourceSettingsVM(matchingSetting, audioSource);

                    // the collection was created on the ui thread
                    await _uiDispatcher.InvokeAsync(() => _settingsWindowVm.AudioSourceSettingsVM.Add(viewModel));
                }
                else
                {
                    var newSettingsModel = new AudioSourceSettings { AudioSourceName = audioSource.Name };
                    _audioSourceSettingsModel.Add(newSettingsModel);
                    var newViewModel = new AudioSourceSettingsVM(newSettingsModel, audioSource);
                    await _uiDispatcher.InvokeAsync(() => _settingsWindowVm.AudioSourceSettingsVM.Add(newViewModel));
                }
            }

            // If user was using this audio source last, then automatically activate it
            var savedAudioSourceName = _appSettings.AudioSource;
            if (savedAudioSourceName == null || audioSource.Name != savedAudioSourceName)
            {
                return;
            }

            await HandleAudioSourceContextMenuItemClick(menuItem).ConfigureAwait(false);
        }
    }
}
