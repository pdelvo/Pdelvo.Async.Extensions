using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdelvo.Async.Extensions.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.ReadLine();
        }

        static async void Test()
        {
            //string result;
            //var enumerator = new TaskEnumerator<string>( TestTask(5000), TestTask(1000), TestTask(2000), TestTask(3000));
            //while (enumerator.MoreValues)
            //{
            //    result = await enumerator.GetNextValueAsync();
            //    Console.WriteLine(result);
            //}

            var timer = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            var links = timer.Select(a => "http://tycho.usno.navy.mil/cgi-bin/time.pl?n=" + a);


            var files = links.SelectAsync(async a =>
            {
                var httpClient = new WebClient();
                return await httpClient.DownloadStringTaskAsync(new Uri(a));
            });


            files.ForEach<string>(m => Console.WriteLine(m));
        }

        static async Task<string> TestTask(int delay)
        {
            await Task.Delay(delay);
            return "Waited: " + delay;
        }
    }
}
