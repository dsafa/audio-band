using System;
using System.Collections.Generic;
using System.Linq;
using AudioBand.Logging;
using NLog;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrate settings from one version to another.
    /// </summary>
    internal static class Migration
    {
        // Assume that migrations can be applied in order.
        private static readonly List<(string version, ISettingsMigrator migrator)> MigrationsList = new List<(string, ISettingsMigrator)>
        {
            ("0.1", new V1ToV2()),
            ("2", new V2ToV3()),
            ("3", new IdentityMigrator()),
        };

        private static readonly ILogger Logger = AudioBandLogManager.GetLogger(typeof(Migration).FullName);

        /// <summary>
        /// Migrate settings to new version.
        /// </summary>
        /// <typeparam name="TNew">Type of the new settings.</typeparam>
        /// <param name="oldSettings">The old settings.</param>
        /// <param name="oldVersion">The version of the old settings.</param>
        /// <param name="newVersion">The version of the new settings.</param>
        /// <returns>New settings.</returns>
        public static TNew MigrateSettings<TNew>(object oldSettings, string oldVersion, string newVersion)
        {
            if (oldVersion == newVersion)
            {
                return (TNew)oldSettings;
            }

            var plan = FindPlan(oldVersion, newVersion);
            if (plan == null || !plan.Any())
            {
                throw new ArgumentException($"No migration plan from {oldVersion} to {newVersion}");
            }

            Logger.Debug("Found old settings v{old}. Migrating settings using {plan}", oldVersion, string.Join("->", plan));

            object settings = plan.Aggregate(oldSettings, (current, settingsMigrator) => settingsMigrator.MigrateSetting(current));
            return (TNew)settings;
        }

        private static List<ISettingsMigrator> FindPlan(string from, string to)
        {
            var startIndex = MigrationsList.FindIndex(m => m.version == from);
            var endIndex = MigrationsList.FindIndex(m => m.version == to);
            if (startIndex == -1 || endIndex == -1)
            {
                return null;
            }

            if (endIndex < startIndex)
            {
                Logger.Error("End index less than start index");
                return null;
            }

            return MigrationsList.GetRange(startIndex, endIndex - startIndex).Select(m => m.migrator).ToList();
        }
    }
}
