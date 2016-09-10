using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroXStats.models
{
    public class Death : Entity
    {
        [BsonElement("killer")]
        public string Killer { get; set; }

        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("side")]
        public string Side { get; set; }
    }
}
