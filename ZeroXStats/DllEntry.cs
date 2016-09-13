using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZeroXStats
{
    public static class DllEntry
    {
        private static ZeroXStats stats = new ZeroXStats();

        [DllExport("_RVExtension@12", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        public static void RVExtension(StringBuilder output, int outputSize, [MarshalAs(UnmanagedType.LPStr)] string input)
        {
            var signature = input.Split(new char[] { ' ' }, 2);
            try
            {
                switch (signature.First())
                {
                    case "init":
                        Console.WriteLine("Initializing stats extension...");
                        var config = LoadConfig();
                        stats.Initialize(config["baseUrl"]);
                        output.Append("STARTED");
                        break;
                    case "shutdown":
                        stats.Shutdown();
                        output.Append("STOPPED");
                        break;
                    default:
                        Console.WriteLine("Handling: " + input);
                        stats.Write(signature.First(), JObject.Parse(signature.Last()));
                        output.Append("OK");
                        break;
                }
            }
            catch (Exception e)
            {
                output.Append("ERROR[" + e.Message + "]");
            }
        }

        private static IDictionary<string, string> LoadConfig()
        {
            using (StreamReader reader = new StreamReader("ZeroXStats.json"))
            {
                string config = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(config);
            }
        }
    }
}
