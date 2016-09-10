using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroXStats.models;
using EntityQueue = System.Collections.Concurrent.ConcurrentQueue<ZeroXStats.models.Entity>;

namespace ZeroXStats
{
    public class ZeroXStatsWorker
    {
        private volatile bool stop = false;
        private IMongoClient mongo;
        private IMongoDatabase database;
        private Dictionary<string, IMongoCollection<BsonDocument>> collections;
        private ConcurrentDictionary<string, List<BsonDocument>> grid;

        public ZeroXStatsWorker(string hostname)
        {
            var clientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(hostname)
            };

            mongo = new MongoClient(clientSettings);
            database = mongo.GetDatabase("ZeroXStats");
            collections = new Dictionary<string, IMongoCollection<BsonDocument>>();
            grid = new ConcurrentDictionary<string, List<BsonDocument>>();
        }

        public void Run()
        {
            while (!stop)
            {
                Thread.Sleep(5000);
                foreach (var entry in grid)
                {
                    if (!collections.ContainsKey(entry.Key))
                    {
                        collections.Add(entry.Key, database.GetCollection<BsonDocument>(entry.Key));
                    }

                    var collection = collections[entry.Key];
                    var queue = grid[entry.Key];
                    collection.InsertMany(queue);
                }
            }
        }

        public void Write<T>(T item) where T : Entity
        {
            var collection = typeof(T).Name.ToLower() + "s";
            grid.GetOrAdd(collection, new List<BsonDocument>()).Add(item.ToBsonDocument());
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
