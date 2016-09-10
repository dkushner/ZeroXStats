using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroXStats.models
{
    public class Config
    {
        [JsonProperty("hostname")]
        public String Hostname { get; set; }
    }
}
