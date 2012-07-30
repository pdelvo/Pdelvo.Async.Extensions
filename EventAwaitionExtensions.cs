using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdelvo.Async.Extensions
{
    public static class EventAwaitionExtensions
    {
    }

    public class EventAwaiter<T> where T : EventArgs
    {
        public EventHandler<T> Bind()
        {
            return (s, e) =>
            {

            };
        }
    }
}
