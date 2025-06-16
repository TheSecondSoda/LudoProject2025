using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public class AsyncTaskManager
    {
        private readonly Queue<Func<Task>> taskQueue = new Queue<Func<Task>>();

        public void EnqueueTask(Func<Task> taskFunc)
        {
            taskQueue.Enqueue(taskFunc);
        }


        public async void Update()
        {
            if (taskQueue.Count > 0)
            {
                var task = taskQueue.Dequeue();
                try
                {
                    await task(); // await the async work
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Async task error: " + ex.Message);
                }
            }
        }

    }
}
