using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyConnector
{
    public class SpotifyTrackProgress : IObservable<int>
    {
        private readonly List<IObserver<int>> _observers = new List<IObserver<int>>();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _observers.Add(observer);

            return new SpotifyTrackProgessUnsubscriber<int>(_observers, observer);
        }
    }

    public class SpotifyTrackProgessUnsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public SpotifyTrackProgessUnsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null)
            {
                _observers.Remove(_observer);
            }
        }
    }
}
