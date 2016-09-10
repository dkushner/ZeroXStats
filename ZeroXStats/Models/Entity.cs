using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroXStats.models
{
    [BsonIgnoreExtraElements]
    public class Entity : BsonDocument
    {
        public ObjectId Id { get; set; }
    }
}
