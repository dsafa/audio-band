using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AudioBand.Logging;
using AudioBand.Settings;
using NLog;
using Octokit;

namespace AudioBand
{
    /// <summary>
    /// Helper to interact with the GitHub api.
    /// </summary>
    public class GitHubHelper
    {
        private IAppSettings _settings;
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<GitHubHelper>();
        private GitHubClient _client = new GitHubClient(new ProductHeaderValue("AudioBand"));

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubHelper"/> class.
        /// </summary>
        public GitHubHelper(IAppSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Gets the Download url of the latest release.
        /// </summary>
        /// <returns>A string with a Url of the latest release download.</returns>
        public async Task<string> GetLatestDownloadUrlAsync()
        {
            var release = await GetLatestRelease();
            return release.Assets.FirstOrDefault(x => x.Name == "AudioBand.msi")?.BrowserDownloadUrl;
        }

        /// <summary>
        /// Gets whether the user is currently running the latest version of AudioBand.
        /// </summary>
        /// <returns>Whether the user is on the latest version.</returns>
        public async Task<bool> IsOnLatestVersionAsync()
        {
            try
            {
                var currentVersion = GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                string latestVersion = "";

                latestVersion = (await GetLatestRelease()).Name.Split(' ')[1];

                // custom build or forgot to change version on compile, exclude from updates
                if (currentVersion == "$version$")
                {
                    return true;
                }

                var current = CreateSemanticVersion(currentVersion);
                var latest = CreateSemanticVersion(latestVersion);

                if (latest.Major > current.Major
                || (latest.Major == current.Major && latest.Minor > current.Minor)
                || (latest.Major == current.Major && latest.Minor == current.Minor && latest.Patch > current.Patch))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                Logger.Warn("Could not check for updates, request to GitHub failed.");
                return false;
            }
        }

        private async Task<Release> GetLatestRelease()
        {
            try
            {
                if (_settings.AudioBandSettings.OptInForPreReleases)
                {
                    return (await _client.Repository.Release.GetAll("AudioBand", "AudioBand"))[0];
                }
                else
                {
                    return await _client.Repository.Release.GetLatest("AudioBand", "AudioBand");
                }
            }
            catch (Exception)
            {
                Logger.Warn("Could not check for updates, request to GitHub failed.");
                return null;
            }
        }

        private SemanticVersion CreateSemanticVersion(string version)
        {
            var regex = Regex.Match(version, "\\d+\\.\\d+\\.\\d+");

            if (!regex.Success)
            {
                return new SemanticVersion(0, 0, 0);
            }

            version = version.StartsWith("v") ? version.Remove(0, 1) : version;
            var splits = version.Split('.');
            return new SemanticVersion(int.Parse(splits[0]), int.Parse(splits[1]), int.Parse(splits[2]));
        }

        private struct SemanticVersion
        {
            public SemanticVersion(int major, int minor, int patch)
            {
                Major = major;
                Minor = minor;
                Patch = patch;
            }

            public int Major;

            public int Minor;

            public int Patch;
        }
    }
}
