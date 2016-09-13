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
        private static Dictionary<string, string> endpoints = new Dictionary<string, string>()
        {
            { "operation", "api/operations" },
            { "hit", "api/hits" }
        };

        private volatile bool stop = false;
        private ConcurrentQueue<KeyValuePair<string, JObject>> pending;
        private WebClient client;

        public ZeroXStatsWorker(string baseUrl)
        {
            this.pending = new ConcurrentQueue<KeyValuePair<string, JObject>>();
            this.client = new WebClient()
            {
                BaseAddress = baseUrl,
                Headers = new WebHeaderCollection()
                {
                    { "Accept", "application/vnd.api+json" }
                }
            };
        }

        public void Run()
        {
            while (!stop)
            {
                Thread.Sleep(100);
                while (!pending.IsEmpty)
                {
                    KeyValuePair<string, JObject> entry;
                    if (pending.TryDequeue(out entry))
                    {
                        try
                        {
                            client.UploadString(endpoints[entry.Key], JsonConvert.SerializeObject(entry.Value));
                        }
                        catch (WebException e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
        }

        public void Write(string type, JObject item)
        {
            // Hack in an accurate timestamp because SQF is awful.
            if ("operation".Equals(type))
            {
                Console.WriteLine("Sending operation info.");
                var settings = new JsonSerializerSettings()
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };

                var timestamp = DateTime.UtcNow.ToString("o");
                item.Add("started", timestamp);
            }

            // Bundle everything up into JSON-API format.
            var root = new JObject()
            {
                {
                    "data", new JObject()
                    {
                        { "type", type + "s" },
                        { "attributes", item }
                    }
                }
            };
            pending.Enqueue(new KeyValuePair<string, JObject>(type, root));
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
