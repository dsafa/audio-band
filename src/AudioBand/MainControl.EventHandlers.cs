﻿using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioBand.AudioSource;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.ViewModels;
using AudioBand.Views.Winforms;
using CSDeskBand.ContextMenu;

namespace AudioBand
{
    partial class MainControl
    {
        private static readonly object _audiosourceListLock = new object();

        private void AlbumArtOnMouseLeave(object o, EventArgs args)
        {
            AlbumArtPopup.Hide(this);
        }

        private void AlbumArtOnMouseHover(object o, EventArgs args)
        {
            AlbumArtPopup.Show(this, TaskbarInfo, ScalingFactor);
        }

        private async void PlayPauseButtonOnClick(object sender, MouseEventArgs eventArgs)
        {
            if (!eventArgs.Button.HasFlag(MouseButtons.Left))
            {
                return;
            }

            if (_track.IsPlaying)
            {
                await (_currentAudioSource?.PauseTrackAsync() ?? Task.CompletedTask);
            }
            else
            {
                await (_currentAudioSource?.PlayTrackAsync() ?? Task.CompletedTask);
            }
        }

        private async void PreviousButtonOnClick(object sender, MouseEventArgs eventArgs)
        {
            if (!eventArgs.Button.HasFlag(MouseButtons.Left))
            {
                return;
            }

            await (_currentAudioSource?.PreviousTrackAsync() ?? Task.CompletedTask);
        }

        private async void NextButtonOnClick(object sender, MouseEventArgs eventArgs)
        {
            if (!eventArgs.Button.HasFlag(MouseButtons.Left))
            {
                return;
            }

            await (_currentAudioSource?.NextTrackAsync() ?? Task.CompletedTask);
        }

        private void SettingsMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            OpenSettingsWindow();
        }

        private async void AudioSourceMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            await HandleAudioSourceContextMenuItemClick(sender as DeskBandMenuAction).ConfigureAwait(false);
        }

        private async void AudioSourceOnTrackProgressChanged(object o, TimeSpan progress)
        {
            await _uiDispatcher.InvokeAsync(() => { _track.TrackProgress = progress; });
        }

        private async void AudioSourceOnIsPlayingChanged(object sender, bool isPlaying)
        {
            Logger.Debug("Play state changed. Is playing: {playing}", isPlaying);
            await _uiDispatcher.InvokeAsync(() => _track.IsPlaying = isPlaying);
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            if (e == null)
            {
                Logger.Error("TrackInfoChanged event arg is null");
                return;
            }

            Logger.Debug("Track changed. {track}", new { e.Artist, e.TrackName, e.Album, e.TrackLength });

            _uiDispatcher.InvokeAsync(() =>
            {
                _track.AlbumArt = new DrawingImage(e.AlbumArt);
                _track.Artist = e.Artist;
                _track.TrackName = e.TrackName;
                _track.TrackLength = e.TrackLength;
                _track.AlbumName = e.Album;
            });
        }

        private void ViewModelContainerOnSaved(object o, EventArgs eventArgs)
        {
            _appSettings.Save();
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
                Logger.Warn("Action {action} not supported", e.Action);
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
                var matchingSetting = _appSettings.AudioSourceSettings.FirstOrDefault(s => s.AudioSourceName == audioSource.Name);
                if (matchingSetting != null)
                {
                    var viewModel = new AudioSourceSettingsVM(matchingSetting, audioSource);

                    // the collection was created on the ui thread
                    await _uiDispatcher.InvokeAsync(() => _viewModelContainer.AudioSourceSettingsVM.Add(viewModel));
                }
                else
                {
                    var newSettingsModel = new AudioSourceSettings { AudioSourceName = audioSource.Name };
                    _appSettings.AudioSourceSettings.Add(newSettingsModel);
                    var newViewModel = new AudioSourceSettingsVM(newSettingsModel, audioSource);
                    await _uiDispatcher.InvokeAsync(() => _viewModelContainer.AudioSourceSettingsVM.Add(newViewModel));
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

        private async void ProgressBarOnClick(object sender, MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Left))
            {
                return;
            }

            var progressBar = sender as EnhancedProgressBar;
            Debug.Assert(progressBar != null, "Event should be for progress bar");

            if (_currentAudioSource != null)
            {
                await _currentAudioSource.SetPlaybackProgressAsync(progressBar.GetSeekPositionFromMouse());
            }
        }
    }
}
