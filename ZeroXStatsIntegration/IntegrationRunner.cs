using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZeroXStatsIntegration
{
    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dll);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr module, string procedure);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr module);
    }

    public class IntegrationRunner
    {
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate void RVExtension(StringBuilder output, int outputSize, string input);

        public static void Main(string[] args)
        {
            var library = NativeMethods.LoadLibrary(Assembly.Load("ZeroXStats").Location);
            var location = NativeMethods.GetProcAddress(library, "_RVExtension@12");
            RVExtension entry = (RVExtension)Marshal.GetDelegateForFunctionPointer(location, typeof(RVExtension));

            var result = new StringBuilder();

            entry(result, int.MaxValue, "init");
            Console.WriteLine("Init: " + result.ToString());
            result.Clear();

            entry(result, int.MaxValue, "operation { \"map\": \"whatever\" }");
            Console.WriteLine("Operation: " + result.ToString());
            result.Clear();

            string hitJson = @"{
              ""target"": {
                ""id"": ""Aybak"",
                ""tag"": ""O Alpha 1-1:2 REMOTE"",
                ""player"": false,
                ""side"": ""EAST"",
                ""name"": ""Aybak"",
                ""stance"": ""PRONE"",
                ""position"": {
                  ""x"": 9979.62,
                  ""y"": 11765.1,
                  ""z"": 409.871
                },
                ""eye"": {
                  ""x"": -0.990784,
                  ""y"": 0.135454,
                  ""z"": 0
                },
                ""aim"": {
                  ""x"": -0.990781,
                  ""y"": 0.135474,
                  ""z"": 0.0000356216
                }
              },
              ""shooter"": {
                ""id"": ""76561197960945508"",
                ""tag"": ""B Alpha 1-1:1 (_bryan)"",
                ""player"": true,
                ""side"": ""WEST"",
                ""name"": ""_bryan"",
                ""stance"": ""PRONE"",
                ""position"": {
                  ""x"": 10021.1,
                  ""y"": 11777.8,
                  ""z"": 409.872
                },
                ""eye"": {
                  ""x"": -0.955301,
                  ""y"": -0.295625,
                  ""z"": -0.00227984
                },
                ""aim"": {
                  ""x"": -0.955667,
                  ""y"": -0.294449,
                  ""z"": -0.00124363
                }
              },
              ""cause"": ""1676389: tracer_red.p3d"",
              ""magazine"": ""B_65x39_Caseless"",
              ""radius"": 0.220345,
              ""direct"": true,
              ""limb"": ""spine3"",
              ""impact"": {
                ""x"": 9979.38,
                ""y"": 11765.2,
                ""z"": 410.175
              },
              ""velocity"": {
                ""x"": -695.895,
                ""y"": -215.492,
                ""z"": -0.277843
              },
              ""normal"": {
                ""x"": 0.334214,
                ""y"": -0.252938,
                ""z"": 0.907923
              }
            }";
            entry(result, int.MaxValue, "hit " + hitJson);
            Console.WriteLine("Hit: " + result.ToString());
            result.Clear();

            string dischargeJson = @"{
              ""shooter"": {
                ""id"": ""76561197960945508"",
                ""tag"": ""B Alpha 1-1:1 (_bryan)"",
                ""player"": true,
                ""side"": ""WEST"",
                ""name"": ""_bryan"",
                ""stance"": ""STAND"",
                ""position"": {
                  ""x"": 10083.9,
                  ""y"": 11761.5,
                  ""z"": 409.867
                },
                ""eye"": {
                  ""x"": -0.197104,
                  ""y"": 0.980382,
                  ""z"": -0.00134561
                },
                ""aim"": {
                  ""x"": -0.195375,
                  ""y"": 0.980728,
                  ""z"": 0.000864243
                }
              },
              ""weapon"": ""arifle_MX_khk_ACO_Pointer_F"",
              ""muzzle"": ""arifle_MX_khk_ACO_Pointer_F"",
              ""mode"": ""Single"",
              ""ammo"": ""B_65x39_Caseless"",
              ""magazine"": ""30Rnd_65x39_caseless_mag"",
              ""projectile"": ""1675696: tracer_red.p3d""
            }";

            NativeMethods.FreeLibrary(library);
        }
    }
}
