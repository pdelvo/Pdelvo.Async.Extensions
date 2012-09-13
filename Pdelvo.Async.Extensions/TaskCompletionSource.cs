using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdelvo.Async.Extensions
{
    public class TaskCompletionSource
    {
        TaskCompletionSource<object> _innerSource;

        public TaskCompletionSource()
        {
            _innerSource = new TaskCompletionSource<object>();
        }

        public TaskCompletionSource(object state)
        {
            _innerSource = new TaskCompletionSource<object>(state);
        }

        public TaskCompletionSource(TaskCreationOptions options)
        {
            _innerSource = new TaskCompletionSource<object>(options);
        }

        public TaskCompletionSource(object state, TaskCreationOptions options)
        {
            _innerSource = new TaskCompletionSource<object>(state, options);
        }

        public void SetCompleted()
        {
            _innerSource.SetResult(null);
        }

        public bool TrySetCompleted()
        {
            return _innerSource.TrySetResult(null);
        }

        public void SetException(Exception exception)
        {
            _innerSource.SetException(exception);
        }

        public void SetException(IEnumerable<Exception> exceptions)
        {
            _innerSource.SetException(exceptions);
        }

        public void SetCanceled()
        {
            _innerSource.SetCanceled();
        }

        public bool TrySetCanceled()
        {
            return _innerSource.TrySetCanceled();
        }

        public Task Task
        {
            get
            {
                return _innerSource.Task;
            }
        }
    }
}
