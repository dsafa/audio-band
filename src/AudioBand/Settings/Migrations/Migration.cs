using AudioBand.Settings.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrate settings from one version to another
    /// </summary>
    internal static class Migration
    {
        private static readonly Dictionary<(string From, string To), ISettingsMigrator> SupportedMigrations = new Dictionary<(string From, string To), ISettingsMigrator>()
        {
            {("0.1", "2"), new V1ToV2()}
        };

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static TNew MigrateSettings<TNew>(object oldSettings, string oldVersion, string newVersion)
        {
            if (oldVersion == newVersion)
            {
                return (TNew) oldSettings;
            }

            var plan = FindPlan(oldVersion, newVersion);
            if (!plan.Any())
            {
                throw new ArgumentException($"No migration plan from {oldVersion} to {newVersion}");
            }

            Logger.Debug($"Found old settings v{oldVersion}. Migrating settings using {String.Join("->", plan)}");

            object settings = plan.Aggregate(oldSettings, (current, settingsMigrator) => settingsMigrator.MigrateSetting(current));
            return (TNew) settings;
        }

        private static List<ISettingsMigrator> FindPlan(string from, string to)
        {
            return SupportedMigrations.Where(x => x.Key.From == from && x.Key.To == to).Select(x => x.Value).ToList();
        }
    }
}
