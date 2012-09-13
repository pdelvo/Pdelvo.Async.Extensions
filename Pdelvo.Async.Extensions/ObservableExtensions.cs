using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pdelvo.Async.Extensions.Helpers;

namespace Pdelvo.Async.Extensions
{
    public static class ObservableExtensions
    {
        public static ObservableAwaiter<T> GetAwaiter<T>(this IObservable<T> observable)
        {
            return new ObservableAwaiter<T>(observable);
        }

        public static IObservable<TOutput> SelectAsync<TInput, TOutput>(this IObservable<TInput> observable, Func<TInput, Task<TOutput>> callFunc)
        {
            if (observable == null) throw new ArgumentNullException("observable");

            var observer = new AsyncObserver<TInput, TOutput>(callFunc);
            observable.Subscribe(observer);
            return observer;
        }

        public static IObservable<T> Unbuffer<T>(this IObservable<IEnumerable<T>> observable)
        {
            if (observable == null) throw new ArgumentNullException("observable");
            
            var observer = new BufferObserver<T>();
            observable.Subscribe(observer);
            return observer;
        }


        class AsyncObserver<TInput, TOutput> : IObserver<TInput>, IObservable<TOutput>
        {
            List<IObserver<TOutput>> _childObserver = new List<IObserver<TOutput>>();

            Func<TInput, Task<TOutput>> _task;

            public AsyncObserver(Func<TInput, Task<TOutput>> task)
            {
                _task = task;
            }

            public void OnCompleted()
            {
                foreach (var item in _childObserver)
                {
                    item.OnCompleted();
                }
            }

            public void OnError(Exception error)
            {
                foreach (var item in _childObserver)
                {
                    item.OnError(error);
                }
            }

            public async void OnNext(TInput value)
            {
                var result = await _task(value);
                foreach (var item in _childObserver)
                {
                    item.OnNext(result);
                }

            }

            public IDisposable Subscribe(IObserver<TOutput> observer)
            {
                lock (_childObserver)
                    _childObserver.Add(observer);
                return new ObjectDisposer<IObserver<TOutput>>(OnDisposed, observer);
            }

            void OnDisposed(IObserver<TOutput> observer)
            {
                lock (_childObserver)
                {
                    _childObserver.Remove(observer);
                }
            }
        }

        class BufferObserver<T> : IObserver<IEnumerable<T>>, IObservable<T>
        {
            List<IObserver<T>> _childObserver = new List<IObserver<T>>();

            public void OnCompleted()
            {
                foreach (var item in _childObserver)
                {
                    item.OnCompleted();
                }
            }

            public void OnError(Exception error)
            {
                foreach (var item in _childObserver)
                {
                    item.OnError(error);
                }
            }

            public void OnNext(IEnumerable<T> value)
            {
                if (value == null) throw new ArgumentNullException("value");
                foreach (var innerValue in value)
                    foreach (var item in _childObserver)
                    {
                        item.OnNext(innerValue);
                    }

            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                lock (_childObserver)
                    _childObserver.Add(observer);
                return new ObjectDisposer<IObserver<T>>(OnDisposed, observer);
            }

            void OnDisposed(IObserver<T> observer)
            {
                lock (_childObserver)
                {
                    _childObserver.Remove(observer);
                }
            }
        }
    }

    public class ObservableAwaiter<T> : IObserver<T>, INotifyCompletion
    {
        bool _isCompleted;
        Action _moveNextAction;
        ConcurrentQueue<T> _items = new ConcurrentQueue<T>();
        IDisposable _disposable;
        SynchronizationContext _context;

        public ObservableAwaiter(IObservable<T> observable)
        {
            if (observable == null) throw new ArgumentNullException("observable");
            _disposable = observable.Subscribe(this);
        }

        public void OnCompleted(Action continuation)
        {
            _context = SynchronizationContext.Current ?? default(SynchronizationContext);
            _moveNextAction = continuation;
        }
        public T[] GetResult()
        {
            return _items.ToArray();
        }

        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
        }

        public void OnCompleted()
        {
            _disposable.Dispose();
            _isCompleted = true;
            _context.Post(s => _moveNextAction(), null);
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
            _items.Enqueue(value);
        }
    }

}
