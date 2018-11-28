//--------------------------------------------------------//
// MusicBeeIPCSDK C# v2.0.0                               //
// Copyright © Kerli Low 2014                             //
// This file is licensed under the                        //
// BSD 2-Clause License                                   //
// See LICENSE_MusicBeeIPCSDK for more information.       //
//--------------------------------------------------------//

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
public partial class MusicBeeIPC
{
    [DllImport("user32")]
    public static extern IntPtr SendMessage(IntPtr hwnd, uint wMsg, UIntPtr wParam, IntPtr lParam);
    [DllImport("user32")]
    public static extern IntPtr FindWindow(IntPtr lpClassName, string lpWindowName);

    public MusicBeeIPC()
    {
    }

    public bool Probe()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Probe, IntPtr.Zero) != Error.Error;
    }

    public Error PlayPause()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.PlayPause, IntPtr.Zero);
    }

    public Error Play()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Play, IntPtr.Zero);
    }

    public Error Pause()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Pause, IntPtr.Zero);
    }

    public Error Stop()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Stop, IntPtr.Zero);
    }

    public Error StopAfterCurrent()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.StopAfterCurrent, IntPtr.Zero);
    }

    public Error PreviousTrack()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.PreviousTrack, IntPtr.Zero);
    }

    public Error NextTrack()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.NextTrack, IntPtr.Zero);
    }

    public Error StartAutoDj()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.StartAutoDj, IntPtr.Zero);
    }

    public Error EndAutoDj()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.EndAutoDj, IntPtr.Zero);
    }

    public PlayState GetPlayState()
    {
        return (PlayState)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetPlayState, IntPtr.Zero);
    }

    public string GetPlayStateStr()
    {
        switch ((PlayState)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetPlayState, IntPtr.Zero))
        {
            case PlayState.Loading:
                return "Loading";
            case PlayState.Playing:
                return "Playing";
            case PlayState.Paused:
                return "Paused";
            case PlayState.Stopped:
                return "Stopped";
            default:
                return "Undefined";
        }
    }

    public int GetPosition()
    {
        return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetPosition, IntPtr.Zero);
    }

    public Error SetPosition(int position)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetPosition, (IntPtr)position);
    }

    public int Position
    {
        get
        {
            return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetPosition, IntPtr.Zero);
        }

        set
        {
            SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetPosition, (IntPtr)value);
        }
    }

    // Volume: Value between 0 - 100
    public int GetVolume()
    {
        return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetVolume, IntPtr.Zero);
    }

    // Volume: Value between 0 - 100
    public Error SetVolume(int volume)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetVolume, (IntPtr)volume);
    }

    // Volume: Value between 0 - 100
    public int Volume
    {
        get
        {
            return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetVolume, IntPtr.Zero);
        }

        set
        {
            SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetVolume, (IntPtr)value);
        }
    }

    // Precise volume: Value between 0 - 10000
    public int GetVolumep()
    {
        return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetVolumep, IntPtr.Zero);
    }

    // Precise volume: Value between 0 - 10000
    public Error SetVolumep(int volume)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetVolumep, (IntPtr)volume);
    }

    // Precise volume: Value between 0 - 10000
    public int Volumep
    {
        get
        {
            return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetVolumep, IntPtr.Zero);
        }

        set
        {
            SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetVolumep, (IntPtr)value);
        }
    }

    // Floating point volume: Value between 0.0 - 1.0
    public float GetVolumef()
    {
        return (new FloatInt((int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetVolumef, IntPtr.Zero))).f;
    }

    // Floating point volume: Value between 0.0 - 1.0
    public Error SetVolumef(float volume)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetVolumef, (IntPtr)(new FloatInt(volume)).i);
    }

    // Floating point volume: Value between 0.0 - 1.0
    public float Volumef
    {
        get
        {
            return (new FloatInt((int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetVolumef, IntPtr.Zero))).f;
        }

        set
        {
            SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetVolumef, (IntPtr)(new FloatInt(value)).i);
        }
    }

    public bool GetMute()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetMute, IntPtr.Zero));
    }

    public Error SetMute(bool mute)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetMute, ToIntPtr(mute));
    }

    public bool GetShuffle()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetShuffle, IntPtr.Zero));
    }

    public Error SetShuffle(bool shuffle)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetShuffle, ToIntPtr(shuffle));
    }

    public RepeatMode GetRepeat()
    {
        return (RepeatMode)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetRepeat, IntPtr.Zero);
    }

    public Error SetRepeat(RepeatMode repeat)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetRepeat, (IntPtr)repeat);
    }

    public bool GetEqualiserEnabled()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetEqualiserEnabled, IntPtr.Zero));
    }

    public Error SetEqualiserEnabled(bool enabled)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetEqualiserEnabled, ToIntPtr(enabled));
    }

    public bool GetDspEnabled()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetDspEnabled, IntPtr.Zero));
    }

    public Error SetDspEnabled(bool enabled)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetDspEnabled, ToIntPtr(enabled));
    }

    public bool GetScrobbleEnabled()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetScrobbleEnabled, IntPtr.Zero));
    }

    public Error SetScrobbleEnabled(bool enabled)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetScrobbleEnabled, ToIntPtr(enabled));
    }

    public Error ShowEqualiser()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.ShowEqualiser, IntPtr.Zero);
    }

    public bool GetAutoDjEnabled()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetAutoDjEnabled, IntPtr.Zero));
    }

    public bool GetStopAfterCurrentEnabled()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetStopAfterCurrentEnabled, IntPtr.Zero));
    }

    public Error SetStopAfterCurrentEnabled(bool enabled)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetStopAfterCurrentEnabled, ToIntPtr(enabled));
    }

    public bool GetCrossfade()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetCrossfade, IntPtr.Zero));
    }

    public Error SetCrossfade(bool crossfade)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetCrossfade, ToIntPtr(crossfade));
    }

    public ReplayGainMode GetReplayGainMode()
    {
        return (ReplayGainMode)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetReplayGainMode, IntPtr.Zero);
    }

    public Error SetReplayGainMode(ReplayGainMode mode)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.SetReplayGainMode, (IntPtr)mode);
    }

    public Error QueueRandomTracks(int count)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.QueueRandomTracks, (IntPtr)count);
    }

    public int GetDuration()
    {
        return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetDuration, IntPtr.Zero);
    }

    public string GetFileUrl()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetFileUrl, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetFileProperty(FileProperty fileProperty)
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetFileProperty, (IntPtr)fileProperty);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetFileTag(MetaData field)
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetFileTag, (IntPtr)field);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetLyrics()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetLyrics, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetDownloadedLyrics()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetDownloadedLyrics, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetArtwork()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetArtwork, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetArtworkUrl()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetArtworkUrl, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetDownloadedArtwork()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetDownloadedArtwork, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetDownloadedArtworkUrl()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetDownloadedArtworkUrl, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetArtistPicture(int fadingPercent)
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetArtistPicture, (IntPtr)fadingPercent);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public Error GetArtistPictureUrls(bool localOnly, out string[] urls)
    {
        Error r = Error.Error;

        urls = new string[0];

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetArtistPicture, ToIntPtr(localOnly));

        if (Unpack(lr, out urls))
            r = Error.NoError;

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string GetArtistPictureThumb()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetArtistPictureThumb, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public bool IsSoundtrack()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.IsSoundtrack, IntPtr.Zero));
    }

    public Error GetSoundtrackPictureUrls(bool localOnly, out string[] urls)
    {
        Error r = Error.Error;

        urls = new string[0];

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.GetSoundtrackPictureUrls, ToIntPtr(localOnly));

        if (Unpack(lr, out urls))
            r = Error.NoError;

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public int GetCurrentIndex()
    {
        return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetCurrentIndex, IntPtr.Zero);
    }

    public int GetNextIndex(int offset)
    {
        return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetNextIndex, (IntPtr)offset);
    }

    public bool IsAnyPriorTracks()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.IsAnyPriorTracks, IntPtr.Zero));
    }

    public bool IsAnyFollowingTracks()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.IsAnyFollowingTracks, IntPtr.Zero));
    }

    public Error PlayNow(string fileUrl)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.PlayNow, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error QueueNext(string fileUrl)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.QueueNext, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error QueueLast(string fileUrl)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.QueueLast, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error RemoveAt(int index)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.RemoveAt, (IntPtr)index);
    }

    public Error ClearNowPlayingList()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.ClearNowPlayingList, IntPtr.Zero);
    }

    public Error MoveFiles(int[] fromIndices, int toIndex)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fromIndices, toIndex);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.MoveFiles, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error ShowNowPlayingAssistant()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.ShowNowPlayingAssistant, IntPtr.Zero);
    }

    public bool GetShowTimeRemaining()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetShowTimeRemaining, IntPtr.Zero));
    }

    public bool GetShowRatingTrack()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetShowRatingTrack, IntPtr.Zero));
    }

    public bool GetShowRatingLove()
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetShowRatingLove, IntPtr.Zero));
    }

    public bool GetButtonEnabled(PlayButtonType button)
    {
        return ToBool(SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.GetButtonEnabled, (IntPtr)button));
    }

    public Error Jump(int index)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Jump, (IntPtr)index);
    }

    public Error Search(string query, out string[] result)
    {
        return Search(query, "Contains", new[] { "ArtistPeople", "Title", "Album" }, out result);
    }

    public Error Search(string query, string comparison, string[] fields, out string[] result)
    {
        result = new string[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Search, cdPtr);

            if (Unpack(lr, out result))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string SearchFirst(string query)
    {
        return SearchFirst(query, "Contains", new[] { "ArtistPeople", "Title", "Album" });
    }

    public string SearchFirst(string query, string comparison, string[] fields)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.SearchFirst, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error SearchIndices(string query, out int[] result)
    {
        return SearchIndices(query, "Contains", new[] { "ArtistPeople", "Title", "Album" }, out result);
    }

    public Error SearchIndices(string query, string comparison, string[] fields, out int[] result)
    {
        result = new int[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.SearchIndices, cdPtr);

            if (Unpack(lr, out result))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public int SearchFirstIndex(string query)
    {
        return SearchFirstIndex(query, "Contains", new[] { "ArtistPeople", "Title", "Album" });
    }

    public int SearchFirstIndex(string query, string comparison, string[] fields)
    {
        int result = -1;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            result = (int)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.SearchFirstIndex, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error SearchAndPlayFirst(string query)
    {
        return SearchAndPlayFirst(query, "Contains", new[] { "ArtistPeople", "Title", "Album" });
    }

    public Error SearchAndPlayFirst(string query, string comparison, string[] fields)
    {
        Error result = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            result = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.SearchAndPlayFirst, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string NowPlayingList_GetListFileUrl(int index)
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.NowPlayingList_GetListFileUrl, (IntPtr)index);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string NowPlayingList_GetFileProperty(int index, FileProperty fileProperty)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(index, (int)fileProperty);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.NowPlayingList_GetFileProperty, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string NowPlayingList_GetFileTag(int index, MetaData field)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(index, (int)field);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.NowPlayingList_GetFileTag, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error NowPlayingList_QueryFiles(string query)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.NowPlayingList_QueryFiles, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string NowPlayingList_QueryGetNextFile()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.NowPlayingList_QueryGetNextFile, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string NowPlayingList_QueryGetAllFiles()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.NowPlayingList_QueryGetAllFiles, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public Error NowPlayingList_QueryFilesEx(string query, out string[] result)
    {
        result = new string[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.NowPlayingList_QueryFilesEx, cdPtr);

            if (Unpack(lr, out result))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error NowPlayingList_PlayLibraryShuffled()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.NowPlayingList_PlayLibraryShuffled,
                                  IntPtr.Zero);
    }

    public int NowPlayingList_GetItemCount()
    {
        return (int)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.NowPlayingList_GetItemCount, IntPtr.Zero);
    }

    public string Playlist_GetName(string playlistUrl)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Playlist_GetName, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public PlaylistFormat Playlist_GetType(string playlistUrl)
    {
        PlaylistFormat r = PlaylistFormat.Unknown;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (PlaylistFormat)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_GetType, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public bool Playlist_IsInList(string playlistUrl, string filename)
    {
        bool r = false;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl, filename);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = ToBool(SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_IsInList, cdPtr));
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Playlist_QueryPlaylists()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Playlist_QueryPlaylists, IntPtr.Zero);
    }

    public string Playlist_QueryGetNextPlaylist()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Playlist_QueryGetNextPlaylist, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public Error Playlist_QueryFiles(string playlistUrl)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_QueryFiles, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Playlist_QueryGetNextFile()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Playlist_QueryGetNextFile, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string Playlist_QueryGetAllFiles()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Playlist_QueryGetAllFiles, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public Error Playlist_QueryFilesEx(string playlistUrl, out string[] result)
    {
        result = new string[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Playlist_QueryFilesEx, cdPtr);

            if (Unpack(lr, out result))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Playlist_CreatePlaylist(string folderName, string playlistName, string[] filenames)
    {
        string r = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(folderName, playlistName, filenames);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Playlist_CreatePlaylist, cdPtr);

            Unpack(lr, out r);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Playlist_DeletePlaylist(string playlistUrl)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_DeletePlaylist, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Playlist_SetFiles(string playlistUrl, string[] filenames)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl, filenames);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_SetFiles, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Playlist_AppendFiles(string playlistUrl, string[] filenames)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl, filenames);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_AppendFiles, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Playlist_RemoveAt(string playlistUrl, int index)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl, index);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_RemoveAt, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Playlist_MoveFiles(string playlistUrl, int[] fromIndices, int toIndex)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl, fromIndices, toIndex);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_MoveFiles, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Playlist_PlayNow(string playlistUrl)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_PlayNow, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public int Playlist_GetItemCount(string playlistUrl)
    {
        int r = 0;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(playlistUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (int)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Playlist_GetItemCount, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Library_GetFileProperty(string fileUrl, FileProperty fileProperty)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl, (int)fileProperty);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetFileProperty, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string Library_GetFileTag(string fileUrl, MetaData field)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl, (int)field);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetFileTag, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error Library_SetFileTag(string fileUrl, MetaData field, string value)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl, (int)field, value);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Library_SetFileTag, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Library_CommitTagsToFile(string fileUrl)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Library_CommitTagsToFile, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Library_GetLyrics(string fileUrl, LyricsType type)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl, (int)type);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetLyrics, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string Library_GetArtwork(string fileUrl, int index)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl, index);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetArtwork, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string Library_GetArtworkUrl(string fileUrl, int index)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl, index);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetArtworkUrl, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string Library_GetArtistPicture(string artistName, int fadingPercent, int fadingColor)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(artistName, fadingPercent, fadingColor);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetArtistPicture, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error Library_GetArtistPictureUrls(string artistName, bool localOnly, out string[] urls)
    {
        urls = new string[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(artistName, localOnly);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetArtistPictureUrls, cdPtr);

            if (Unpack(lr, out urls))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Library_GetArtistPictureThumb(string artistName)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(artistName);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_GetArtistPictureThumb, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string Library_AddFileToLibrary(string fileUrl, LibraryCategory category)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(fileUrl, (int)category);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_AddFileToLibrary, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error Library_QueryFiles(string query)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Library_QueryFiles, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Library_QueryGetNextFile()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Library_QueryGetNextFile, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public string Library_QueryGetAllFiles()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Library_QueryGetAllFiles, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public Error Library_QueryFilesEx(string query, out string[] result)
    {
        result = new string[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_QueryFilesEx, cdPtr);

            if (Unpack(lr, out result))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Library_QuerySimilarArtists(string artistName, double minimumArtistSimilarityRating)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(artistName, minimumArtistSimilarityRating);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_QuerySimilarArtists, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error Library_QueryLookupTable(string keyTags, string valueTags, string query)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(keyTags, valueTags, query);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Library_QueryLookupTable, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Library_QueryGetLookupTableValue(string key)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(key);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_QueryGetLookupTableValue, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error Library_Jump(int index)
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Library_Jump, (IntPtr)index);
    }

    public Error Library_Search(string query, out string[] result)
    {
        return Library_Search(query, "Contains", new[] { "ArtistPeople", "Title", "Album" }, out result);
    }

    public Error Library_Search(string query, string comparison, string[] fields, out string[] result)
    {
        result = new string[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_Search, cdPtr);

            if (Unpack(lr, out result))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public string Library_SearchFirst(string query)
    {
        return Library_SearchFirst(query, "Contains", new[] { "ArtistPeople", "Title", "Album" });
    }

    public string Library_SearchFirst(string query, string comparison, string[] fields)
    {
        string result = "";

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_SearchFirst, cdPtr);

            Unpack(lr, out result);

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error Library_SearchIndices(string query, out int[] result)
    {
        return Library_SearchIndices(query, "Contains", new[] { "ArtistPeople", "Title", "Album" }, out result);
    }

    public Error Library_SearchIndices(string query, string comparison, string[] fields, out int[] result)
    {
        result = new int[0];

        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            IntPtr hwnd = FindHwnd();

            IntPtr lr = SendMessage(hwnd, WM_COPYDATA, (UIntPtr)Command.Library_SearchIndices, cdPtr);

            if (Unpack(lr, out result))
                r = Error.NoError;

            SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public int Library_SearchFirstIndex(string query)
    {
        return Library_SearchFirstIndex(query, "Contains", new[] { "ArtistPeople", "Title", "Album" });
    }

    public int Library_SearchFirstIndex(string query, string comparison, string[] fields)
    {
        int result = -1;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            result = (int)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Library_SearchFirstIndex, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public Error Library_SearchAndPlayFirst(string query)
    {
        return Library_SearchAndPlayFirst(query, "Contains", new[] { "ArtistPeople", "Title", "Album" });
    }

    public Error Library_SearchAndPlayFirst(string query, string comparison, string[] fields)
    {
        Error result = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(query, comparison, fields);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            result = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Library_SearchAndPlayFirst, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return result;
    }

    public string Setting_GetFieldName(MetaData field)
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Setting_GetFieldName, (IntPtr)field);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public DataType Setting_GetDataType(MetaData field)
    {
        return (DataType)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Setting_GetDataType, (IntPtr)field);
    }

    public IntPtr Window_GetHandle()
    {
        return SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Window_GetHandle, IntPtr.Zero);
    }

    public Error Window_Close()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Window_Close, IntPtr.Zero);
    }

    public Error Window_Restore()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Window_Restore, IntPtr.Zero);
    }

    public Error Window_Minimize()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Window_Minimize, IntPtr.Zero);
    }

    public Error Window_Maximize()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Window_Maximize, IntPtr.Zero);
    }

    public Error Window_Move(int x, int y)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(x, y);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Window_Move, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Window_Resize(int w, int h)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(w, h);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.Window_Resize, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    public Error Window_BringToFront()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Window_BringToFront, IntPtr.Zero);
    }

    public Error Window_GetPosition(out int x, out int y)
    {
        Error r = Error.Error;

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Window_GetPosition, IntPtr.Zero);

        if (Unpack(lr, out x, out y))
            r = Error.NoError;

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public Error Window_GetSize(out int w, out int h)
    {
        Error r = Error.Error;

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.Window_GetSize, IntPtr.Zero);

        if (Unpack(lr, out w, out h))
            r = Error.NoError;

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public MusicBeeVersion GetMusicBeeVersion()
    {
        return (MusicBeeVersion)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.MusicBeeVersion, IntPtr.Zero);
    }

    public string GetMusicBeeVersionStr()
    {
        switch ((MusicBeeVersion)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.MusicBeeVersion, IntPtr.Zero))
        {
            case MusicBeeVersion.v2_0:
                return "2.0";
            case MusicBeeVersion.v2_1:
                return "2.1";
            case MusicBeeVersion.v2_2:
                return "2.2";
            case MusicBeeVersion.v2_3:
                return "2.3";
            default:
                return "Unknown";
        }
    }

    public string GetPluginVersionStr()
    {
        string r = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.PluginVersion, IntPtr.Zero);

        Unpack(lr, out r);

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        return r;
    }

    public Error GetPluginVersion(out int major, out int minor)
    {
        major = minor = 0;

        Error r = Error.Error;
        string v = "";

        IntPtr hwnd = FindHwnd();

        IntPtr lr = SendMessage(hwnd, WM_USER, (UIntPtr)Command.PluginVersion, IntPtr.Zero);

        if (Unpack(lr, out v))
            r = Error.NoError;

        SendMessage(hwnd, WM_USER, (UIntPtr)Command.FreeLRESULT, lr);

        if (r == Error.NoError)
        {
            string[] split = v.Split('.');

            try
            {
                major = Int32.Parse(split[0]);
                minor = Int32.Parse(split[1]);
            }
            catch
            {
                r = Error.Error;
            }
        }

        return r;
    }

    public Error Test()
    {
        return (Error)SendMessage(FindHwnd(), WM_USER, (UIntPtr)Command.Test, IntPtr.Zero);
    }

    public Error MessageBox(string text, string caption)
    {
        Error r = Error.Error;

        IntPtr cdPtr = IntPtr.Zero;

        COPYDATASTRUCT cds = new COPYDATASTRUCT();

        try
        {
            cds = Pack(text, caption);

            cdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cds));

            Marshal.StructureToPtr(cds, cdPtr, false);

            r = (Error)SendMessage(FindHwnd(), WM_COPYDATA, (UIntPtr)Command.MessageBox, cdPtr);
        }
        finally
        {
            Free(ref cds);
            Marshal.FreeHGlobal(cdPtr);
        }

        return r;
    }

    private IntPtr FindHwnd()
    {
        return FindWindow(IntPtr.Zero, "MusicBee IPC Interface");
    }

    private IntPtr ToIntPtr(bool b)
    {
        return b ? (IntPtr)Bool.True : (IntPtr)Bool.False;
    }

    private bool ToBool(IntPtr i)
    {
        return i == (IntPtr)Bool.False ? false : true;
    }
}
