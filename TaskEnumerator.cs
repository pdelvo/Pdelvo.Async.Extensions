using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdelvo.Async.Extensions
{
    /// <summary>
    /// A Enumerator to wait at the next ended task and enumerate through all
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskEnumerator<T>
    {
        List<Task<T>> _tasks;
        Task<T>[] _allTasks;

        public TaskEnumerator(params Task<T>[] tasks)
        {
            _tasks = new List<Task<T>>(tasks);
            _allTasks = _tasks.ToArray();
        }

        public bool MoreValues
        {
            get
            {
                return _tasks.Count > 0;
            }
        }

        public async Task<T> GetNextValueAsync()
        {
            if (_tasks.Count == 0) throw new InvalidOperationException("All tasks are allready finished and received");
            var result = await Task.WhenAny(_tasks);
            _tasks.Remove(result);
            return await result;
        }

        public Task<T[]> GetAllValuesAsync()
        {
            return Task.WhenAll(_allTasks);
        }

        public Task<T[]> GetUnfinishedValuesAsync()
        {
            return Task.WhenAll(_tasks);
        }
    }
}
