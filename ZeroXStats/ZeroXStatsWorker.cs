using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroXStats
{
    public class ZeroXStatsWorker
    {
        private volatile bool stop = false;
        private ConcurrentDictionary<string, Queue<JObject>> pending;
        private WebClient client;

        public ZeroXStatsWorker(string baseUrl)
        {
            this.pending = new ConcurrentDictionary<string, Queue<JObject>>();
            this.client = new WebClient()
            {
                BaseAddress = baseUrl
            };
        }

        public void Run()
        {
            while (!stop)
            {
                Thread.Sleep(100);
                foreach (var entry in pending)
                {
                    var endpoint = new Uri(entry.Key);
                    var collection = entry.Value;
                    foreach (var item in collection)
                    {
                        client.UploadString(endpoint, JsonConvert.SerializeObject(item));
                    }
                }
            }
        }

        public void Write(string type, string item)
        {
            // Endpoints are pluralized.
            var collection = type.ToLower() + "s";
            var parsed = JObject.Parse(item);
            pending.GetOrAdd(collection, new Queue<JObject>()).Enqueue(parsed);
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
