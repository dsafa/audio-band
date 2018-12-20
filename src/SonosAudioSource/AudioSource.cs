using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using AudioBand.AudioSource;
using Image = System.Drawing.Image;
using Timer = System.Timers.Timer;

namespace SonosAudioSource
{
	public class AudioSource : IAudioSource
    {
		private readonly Timer _checkSonosTimer = new Timer(1000);
		private HttpClient _httpClient = new HttpClient();
		private TimeSpan _trackProgress;
		private TimeSpan _lastTrackProgress;
		private TrackInfo _currentTrackInfo;
		private bool _isActive;
		private bool _isPlaying;
		private string _clientIp;
		private string _clientPort;
		private string _defaultClientPort = "1400";

		public AudioSource()
		{
			_checkSonosTimer.AutoReset = false;
			_checkSonosTimer.Elapsed += CheckSonosTimerOnElapsed;
		}

		public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

		public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

#pragma warning disable 00067 // The event is never used
		public event EventHandler<SettingChangedEventArgs> SettingChanged;
#pragma warning restore 00067 // The event is never used

		public string Name { get; } = "Sonos";

        public IAudioSourceLogger Logger { get; set; }

		[AudioSourceSetting("Sonos IP")]
		public string ClientIp
		{
			get => _clientIp;
			set
			{
				if (value == _clientIp)
				{
					return;
				}

				_clientIp = value;
			}
		}

		[AudioSourceSetting("Sonos Port (Default: 1400)")]
		public string ClientPort
		{
			get => _clientPort;
			set
			{
				if (value == _clientPort)
				{
					return;
				}

				_clientPort = value;
			}
		}

		public Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			_isActive = true;
			_checkSonosTimer.Start();

			return Task.CompletedTask;
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			_isActive = false;
			_checkSonosTimer.Stop();

			return Task.CompletedTask;
        }

        public Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
			DoWebRequest("/MediaRenderer/AVTransport/Control",
					"\"urn:schemas-upnp-org:service:AVTransport:1#Play\"",
					"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
					"	<s:Body>\n" +
					"		<u:Play xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
					"			<InstanceID>0</InstanceID>\n" +
					"			<Speed>1</Speed>\n" +
					"		</u:Play>\n" +
					"	</s:Body>\n" +
					"</s:Envelope>");
			return Task.CompletedTask;
        }

        public Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
			DoWebRequest("/MediaRenderer/AVTransport/Control",
					"\"urn:schemas-upnp-org:service:AVTransport:1#Pause\"",
					"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
					"	<s:Body>\n" +
					"		<u:Pause xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
					"			<InstanceID>0</InstanceID>\n" +
					"			<Speed>1</Speed>\n" +
					"		</u:Pause>\n" +
					"	</s:Body>\n" +
					"</s:Envelope>");
			return Task.CompletedTask;
        }

        public Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
			DoWebRequest("/MediaRenderer/AVTransport/Control",
					"\"urn:schemas-upnp-org:service:AVTransport:1#Previous\"",
					"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
					"	<s:Body>\n" +
					"		<u:Previous xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
					"			<InstanceID>0</InstanceID>\n" +
					"			<Speed>1</Speed>\n" +
					"		</u:Previous>\n" +
					"	</s:Body>\n" +
					"</s:Envelope>");
			return Task.CompletedTask;
        }

        public Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
			DoWebRequest("/MediaRenderer/AVTransport/Control",
					"\"urn:schemas-upnp-org:service:AVTransport:1#Next\"",
					"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
					"	<s:Body>\n" +
					"		<u:Next xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
					"			<InstanceID>0</InstanceID>\n" +
					"			<Speed>1</Speed>\n" +
					"		</u:Next>\n" +
					"	</s:Body>\n" +
					"</s:Envelope>");
			return Task.CompletedTask;
		}

		private async void CheckSonosTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			try
			{
				if (!_isActive || string.IsNullOrEmpty(_clientIp))
				{
					return;
				}

				TrackInfo newTrackInfo = GetCurrentTrackInfo();
				if (newTrackInfo == null)
				{
					TrackPaused?.Invoke(this, EventArgs.Empty);
					_isPlaying = false;

					return;
				}

				_lastTrackProgress = _trackProgress;
				_trackProgress = newTrackInfo.TrackProgress;
				TrackProgressChanged?.Invoke(this, _trackProgress);

				if (_isPlaying && _lastTrackProgress == _trackProgress)
				{
					TrackPaused?.Invoke(this, EventArgs.Empty);
					_isPlaying = false;

					return;
				}
				else if (!_isPlaying && _lastTrackProgress != _trackProgress)
				{
					TrackPlaying?.Invoke(this, EventArgs.Empty);
					_isPlaying = true;
				}

				if (newTrackInfo != _currentTrackInfo)
				{
					var albumArtImage = await GetAlbumArt(new Uri(newTrackInfo.AlbumArtUri));
					var trackUpdateInfo = new TrackInfoChangedEventArgs
					{
						Artist = newTrackInfo.Artist,
						TrackName = newTrackInfo.Title,
						Album = newTrackInfo.Album,
						TrackLength = newTrackInfo.TrackLength,
						AlbumArt = albumArtImage
					};

					TrackInfoChanged?.Invoke(this, trackUpdateInfo);
					_currentTrackInfo = newTrackInfo;
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
			finally
			{
				_checkSonosTimer.Enabled = true;
			}
		}

		private async Task<Image> GetAlbumArt(Uri albumArtUrl)
		{
			try
			{
				var response = await _httpClient.GetAsync(albumArtUrl);
				if (!response.IsSuccessStatusCode)
				{
					Logger.Warn("Response was not successful when getting album art: " + response);
					return null;
				}

				var stream = await response.Content.ReadAsStreamAsync();
				return Image.FromStream(stream);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				return null;
			}
		}

		private string DoWebRequest(string url, string soapaction, string xmlData)
		{
			try
			{
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://" + _clientIp + ":" + (string.IsNullOrEmpty(_clientPort) ? _defaultClientPort : _clientPort) + url);
				webRequest.Headers.Add("SOAPACTION", soapaction);
				webRequest.Method = "POST";
				webRequest.Timeout = 1000;
				webRequest.ContentType = "text/xml";

				string postData = xmlData;
				byte[] byteArray = Encoding.UTF8.GetBytes(postData);
				webRequest.ContentLength = byteArray.Length;

				Stream dataStream = webRequest.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();

				WebResponse response = webRequest.GetResponse();
				dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				string responseFromServer = reader.ReadToEnd();

				reader.Close();
				response.Close();
				return responseFromServer;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				return e.ToString();
			}
		}

		private TrackInfo GetCurrentTrackInfo()
		{
			string response = DoWebRequest("/MediaRenderer/AVTransport/Control",
								"\"urn:schemas-upnp-org:service:AVTransport:1#GetPositionInfo\"",
								"<s:Envelope \n" +
								"	xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"\n" +
								"	s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"\n" +
								"	>\n" +
								"  <s:Body>\n" +
								"    <u:GetPositionInfo xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
								"      <InstanceID>0</InstanceID>\n" +
								"    </u:GetPositionInfo>\n" +
								"  </s:Body>\n" +
								"</s:Envelope>\n" +
								"<!--MediaRenderer/AVTransport/Control-->");

			try
			{
				XmlDocument responseXML = new XmlDocument();
				responseXML.LoadXml(response);

				string innerText = responseXML.SelectNodes("//TrackMetaData").Item(0).InnerText;
				TimeSpan trackProgress = TimeSpan.Parse(responseXML.SelectNodes("//RelTime").Item(0).InnerText);
				TimeSpan trackLength = TimeSpan.Parse(responseXML.SelectNodes("//TrackDuration").Item(0).InnerText);

				responseXML.LoadXml(innerText);

				string artist = responseXML.GetElementsByTagName("dc:creator").Item(0).InnerText;
				string album = responseXML.GetElementsByTagName("upnp:album").Item(0).InnerText;
				string title = responseXML.GetElementsByTagName("dc:title").Item(0).InnerText;
				string albumArt = responseXML.GetElementsByTagName("upnp:albumArtURI").Item(0).InnerText;

				return new TrackInfo(artist, album, title, albumArt, trackLength, trackProgress);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				return null;
			}
		}
	}
}
