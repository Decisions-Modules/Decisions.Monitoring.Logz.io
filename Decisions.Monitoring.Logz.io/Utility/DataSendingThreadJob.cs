using Decisions.Monitoring.Logz.io.Data;
using DecisionsFramework;
using DecisionsFramework.Design.Flow.QuickAdd;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Utility
{
    abstract class DataSendingThreadJob<T> : IThreadJob
    {
        private readonly List<T> dataList = new List<T>();
        private TimeSpan delay;
        private bool isStarted = false;
        private string queueName;

        public string Id { get; } = Guid.NewGuid().ToString();

        public DataSendingThreadJob(string aQueueName, TimeSpan runDelay)
        {
            delay = runDelay;
            queueName = aQueueName;
        }

        public void AddItem(T data)
        {
            lock (this)
            {
                dataList.Add(data);
                TryToStart();
            }
        }

        public void Run()
        {
            T[] data;
            lock (this)
            {
                isStarted = false;
                data = dataList.ToArray();
                dataList.Clear();
            };
            SendData(data);
        }

        private void TryToStart()
        {
            if (isStarted) return;
            isStarted = true;

            var startTime = DateUtilities.Now().Add(delay);
            ThreadJobService.AddToQueue(startTime, this, queueName);
        }

        protected abstract void SendData(T[] data);

    }
}
