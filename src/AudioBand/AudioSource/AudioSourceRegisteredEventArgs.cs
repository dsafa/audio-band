using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBand.AudioSource
{
    internal class AudioSourceRegisteredEventArgs : EventArgs
    {
        public AudioSourceRegisteredEventArgs(Uri hostServiceUri)
        {
            HostServiceUri = hostServiceUri;
        }

        public Uri HostServiceUri { get; }
    }
}
