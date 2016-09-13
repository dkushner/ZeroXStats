using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroXStats
{
    public class ZeroXStats
    {
        private ZeroXStatsWorker worker;
        private Thread thread;

        public void Initialize(string hostname)
        {
            worker = new ZeroXStatsWorker(hostname);
            thread = new Thread(worker.Run);

            thread.Start();

            while (!thread.IsAlive) { }
        }

        public void Write(string type, JObject item)
        {
            worker.Write(type, item);
        }

        public void Shutdown()
        {
            worker.Stop();
            thread.Join();
        }

        public Boolean Initialized
        {
            get
            {
                return thread.IsAlive;
            }
        }
    }
}
