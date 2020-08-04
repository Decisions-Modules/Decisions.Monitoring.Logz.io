using DecisionsFramework;
using DecisionsFramework.Design.Flow.QuickAdd;
using DecisionsFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Utility
{
    class Buffer<T> : IThreadJob
    {
        private readonly List<T> dataList = new List<T>();
        private TimeSpan delay;
        private bool isStarted = false;
        private readonly Action<T[]> bufferReceiver;

        public Buffer(string threadJobId,TimeSpan runDelay, Action<T[]> aBufferReceiver)
        {
            delay = runDelay;
            Id = threadJobId;
            bufferReceiver = aBufferReceiver;
        }

        public void AddData(T data)
        {
            lock (this)
            {
                dataList.Add(data);
                TryToStart();
            }
        }

        public string Id { get; }

        public void Run()
        {
            T[] data;
            lock (this)
            {
                isStarted = false;
                data = dataList.ToArray();
                dataList.Clear();
            };
            bufferReceiver(data);
        }

        private void TryToStart()
        {
            if (isStarted) return;
            isStarted = true;

            var startTime = DateUtilities.Now().Add(delay);
            ThreadJobService.AddToQueue(startTime, this);
        }
    }
}
