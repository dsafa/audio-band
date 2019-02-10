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

        /// <summary>
        /// Gets the task.
        /// </summary>
        public Task Task => _tcs.Task;

        /// <summary>
        /// Signal that the task is complete.
        /// </summary>
        public void SetResult()
        {
            _tcs.SetResult(null);
        }
    }
}
