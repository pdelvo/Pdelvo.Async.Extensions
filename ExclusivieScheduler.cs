//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Pdelvo.Async.Extensions
//{
//    public class ExclusivieScheduler : TaskScheduler
//    {
//        Task _runner;

//        AwaitableQueue<Task> _taskQueue = new AwaitableQueue<Task>();

//        public ExclusivieScheduler()
//        {
//            _runner = TaskRunner();
//        }

//        async Task TaskRunner()
//        {
//            while (true)
//            {
//                await await _taskQueue.DequeueAsync();
//            }
//        }

//        protected override IEnumerable<Task> GetScheduledTasks()
//        {
//            return _taskQueue.ToArray();
//        }

//        protected override void QueueTask(Task task)
//        {
//            _taskQueue.Enqueue(task);
//        }

//        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
//        {
//            return false;
//        }
//    }
//}
