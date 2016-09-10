using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroXStats.models
{
    public class Vector
    {
        [BsonElement("x")]
        public float X { get; set; }

        [BsonElement("y")]
        public float Y { get; set; }

        [BsonElement("z")]
        public float Z { get; set; }
    }
}
