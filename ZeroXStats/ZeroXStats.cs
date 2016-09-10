using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroXStats.models;

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

        public void Write(string type, string blob)
        {
            switch (type)
            {
                case "hit":
                    worker.Write<Hit>(BsonSerializer.Deserialize<Hit>(blob));
                    break;
                case "death":
                    worker.Write<Death>(BsonSerializer.Deserialize<Death>(blob));
                    break;
                default:
                    break;
            }
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
