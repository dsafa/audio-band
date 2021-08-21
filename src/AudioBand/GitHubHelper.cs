using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octokit;

namespace AudioBand
{
    /// <summary>
    /// Helper to interact with the GitHub api.
    /// </summary>
    public class GitHubHelper
    {
        private GitHubClient _client = new GitHubClient(new ProductHeaderValue("audio-band"));

        /// <summary>
        /// Gets the Download url of the latest release.
        /// </summary>
        public async Task<string> GetLatestDownloadUrlAsync()
        {
            var release = await GetLatestRelease();
            return release.Assets.FirstOrDefault(x => x.Name == "AudioBand.msi")?.BrowserDownloadUrl;
        }

        /// <summary>
        /// Gets whether the user is currently running the latest version of AudioBand.
        /// </summary>
        public async Task<bool> IsOnLatestVersionAsync()
        {
            var currentVersion = GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var latestVersion = (await GetLatestRelease()).Name.Split(' ')[1];

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

        private async Task<Release> GetLatestRelease()
            => (await _client.Repository.Release.GetAll("svr333", "audio-band"))[0];

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

        struct SemanticVersion
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
