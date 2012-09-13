using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdelvo.Async.Extensions.Helpers
{
    public class ObjectDisposer<T> : IDisposable
    {
        T _item;
        Action<T> _disposed;

        public ObjectDisposer(Action<T> disposed, T item)
        {
            _disposed = disposed;
            _item = item;
        }

        public void Dispose()
        {
            _disposed(_item);
        }
    }
}
