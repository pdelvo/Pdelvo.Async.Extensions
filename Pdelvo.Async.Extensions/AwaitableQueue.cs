using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pdelvo.Async.Extensions
{
    [Serializable]
    [ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class AwaitableQueue<T> : IProducerConsumerCollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        ConcurrentQueue<T> _innerQueue = new ConcurrentQueue<T>();
        TaskCompletionSource<T> _completionSource;
        object _taskCompletionSourceLock = new object();

        public void Enqueue(T item)
        {
            lock (_taskCompletionSourceLock)
            {
                if (_completionSource != null)
                    _completionSource.SetResult(item);
                else
                    _innerQueue.Enqueue(item);
            }
        }

        public async Task<T> DequeueAsync()
        {
            T result;
            if (_innerQueue.TryDequeue(out result))
            {
                return result;
            }
            else
            {
                _completionSource = new TaskCompletionSource<T>();
                if (_innerQueue.TryDequeue(out result))
                {
                    _completionSource = null;
                    return result;
                }
                return await _completionSource.Task;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerQueue.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_innerQueue).CopyTo(array, index);
        }

        public void CopyTo(T[] array, int index)
        {
            _innerQueue.CopyTo(array, index);
        }



        public int Count
        {
            get { return _innerQueue.Count; }
        }


        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_innerQueue).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotSupportedException(); }
        }


        public T[] ToArray()
        {
            return _innerQueue.ToArray();
        }

        bool IProducerConsumerCollection<T>.TryAdd(T item)
        {
            return ((IProducerConsumerCollection<T>)_innerQueue).TryAdd(item);
        }

        bool IProducerConsumerCollection<T>.TryTake(out T item)
        {
            return ((IProducerConsumerCollection<T>)_innerQueue).TryTake(out item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
