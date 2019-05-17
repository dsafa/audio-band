using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

// http://www.wpftutorial.net/MergedDictionaryPerformance.html
namespace AudioBand.Resources
{
    /// <summary>
    /// The shared resource dictionary is a specialized resource dictionary
    /// that loads it content only once. If a second instance with the same source
    /// is created, it only merges the resources from the cache.
    /// </summary>
    public class SharedDictionary : ResourceDictionary
    {
        /// <summary>
        /// Internal cache of loaded dictionaries.
        /// </summary>
        private static readonly Dictionary<Uri, ResourceDictionary> Dictionaries = new Dictionary<Uri, ResourceDictionary>();

        /// <summary>
        /// Local member of the source uri.
        /// </summary>
        private Uri _sourceUri;

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        public new Uri Source
        {
            get => _sourceUri;
            set
            {
                _sourceUri = value;

                if (!Dictionaries.ContainsKey(value))
                {
                    // If the dictionary is not yet loaded, load it by setting
                    // the source of the base class
                    base.Source = value;

                    if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                    {
                        Dictionaries.Add(value, this);
                    }
                }
                else
                {
                    // If the dictionary is already loaded, get it from the cache
                    MergedDictionaries.Add(Dictionaries[value]);
                }
            }
        }
    }
}
