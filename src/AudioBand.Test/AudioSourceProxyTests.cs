using AudioBand.AudioSource;
using AudioBand.ServiceContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBand.Test
{
    [TestClass]
    public class AudioSourceProxyTests
    {
        private Mock<IAudioSourceHostService> HostService;
        private Mock<AudioSourceHostCallback> Callback;
        private Mock<IAudioSourceHost> Host;
       
        [TestInitialize]
        public void Init()
        {
            Host = new Mock<IAudioSourceHost>();
            Host.Setup(h => h.GetAudioSourceSettings()).Returns(new List<AudioSourceSettingInfo>());
            Callback = new Mock<AudioSourceHostCallback>();
            HostService = new Mock<IAudioSourceHostService>();
            HostService.SetupGet(h => h.HostCallback).Returns(Callback.Object);
            HostService.Setup(h => h.GetHost(It.IsAny<string>())).Returns(Host.Object);
        }

        [TestMethod]
        public void ProxyCallsReady()
        {
            bool readied = false;
            var proxy = new AudioSourceProxy("test", HostService.Object);
            proxy.Ready += (o, e) => readied = true;

            HostService.Raise(h => h.Restarted += null, null, EventArgs.Empty);

            Assert.IsTrue(readied);
        }

        [TestMethod]
        public async Task ProxyReactivatesWhenHostRestarts()
        {
            var proxy = new AudioSourceProxy("test", HostService.Object);
            await proxy.ActivateAsync(); // successfull

            HostService.Setup(s => s.GetHost(It.IsAny<string>())).Throws(new Exception());
            await proxy.ActivateAsync(); // should fail, Host.activate should not be called

            HostService.Setup(s => s.GetHost(It.IsAny<string>())).Returns(Host.Object);
            HostService.Raise(s => s.Restarted += null, HostService.Object, EventArgs.Empty);

            Host.Verify(h => h.ActivateAsync(), Times.Exactly(2));
        }

        [TestMethod]
        public void ProxyUpdatesSettingsWhenHostRestarts()
        {
            var setting = "Settings";
            var value = "value";

            Host.Setup(h => h.GetAudioSourceSettings()).Returns(new List<AudioSourceSettingInfo> { new AudioSourceSettingInfo { Name = setting } });
            var proxy = new AudioSourceProxy("test", HostService.Object);
            proxy[setting] = value;

            HostService.Raise(s => s.Restarted += null, HostService.Object, EventArgs.Empty);

            Host.Verify(h => h.UpdateSetting(setting, value), Times.Exactly(2));
        }

        private async Task CheckProxyErrorHandle(Func<AudioSourceProxy, Task> action)
        {
            HostService.Setup(s => s.GetHost(It.IsAny<string>())).Throws(new Exception());
            var proxy = new AudioSourceProxy("test", HostService.Object);

            await action(proxy);

            HostService.Verify(h => h.Restart(), Times.Once);
        }

        [TestMethod]
        public async Task ProxyHandlesActivateError()
        {
            await CheckProxyErrorHandle(p => p.ActivateAsync());
        }

        [TestMethod]
        public async Task ProxyHandlesDeactivateError()
        {
            await CheckProxyErrorHandle(p => p.DeactivateAsync());
        }

        [TestMethod]
        public async Task ProxyHandlesNextTrackError()
        {
            await CheckProxyErrorHandle(p => p.NextTrackAsync());
        }

        [TestMethod]
        public async Task ProxyHandlesPreviousTrackError()
        {
            await CheckProxyErrorHandle(p => p.PreviousTrackAsync());
        }

        [TestMethod]
        public async Task ProxyHandlesPlayError()
        {
            await CheckProxyErrorHandle(p => p.PlayTrackAsync());
        }

        [TestMethod]
        public async Task ProxyHandlesPauseError()
        {
            await CheckProxyErrorHandle(p => p.PauseTrackAsync());
        }

        [TestMethod]
        public async Task ProxyCallsRestartOnHostError()
        {
            var proxy = new AudioSourceProxy("test", HostService.Object);
            HostService.Setup(s => s.GetHost(It.IsAny<string>())).Throws(new Exception());

            await proxy.ActivateAsync();

            HostService.Verify(s => s.Restart(), Times.Once);
        }
    }
}
