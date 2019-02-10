using System;
using System.Threading.Tasks;

namespace AudioSourceHost
{
    /// <summary>
    /// Used to run cross app domain tasks.
    /// </summary>
    public class MarshaledTaskCompletionSource : MarshalByRefObject
    {
        private TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();

        public Task Task => _tcs.Task;

        public void SetResult()
        {
            _tcs.SetResult(null);
        }
    }
}
