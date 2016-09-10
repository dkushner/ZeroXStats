using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroXStats.models
{
    public class Hit : Entity
    {
        [BsonElement("target")]
        public string Target { get; set; }

        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("shooter")]
        public string Shooter { get; set; }

        [BsonElement("cause")]
        public string Cause { get; set; }

        [BsonElement("impact")]
        public Vector Impact { get; set; }

        [BsonElement("velocity")]
        public Vector Velocity { get; set; }

        [BsonElement("surface")]
        public Vector Surface { get; set; }

        [BsonElement("radius")]
        public float Radius { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("direct")]
        public bool Direct { get; set; }

        [BsonElement("limbs")]
        public List<string> Limbs { get; set; }
    }
}
