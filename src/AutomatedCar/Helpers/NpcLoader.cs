using AutomatedCar.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Helpers
{
    static class NpcLoader
    {
        public static List<NpcJsonObject> ReadNpcsJson(string fileName = "npcs.json")
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"AutomatedCar.Assets.{fileName}"));

            var npcJsonObjects = JsonConvert.DeserializeObject<List<NpcJsonObject>>(reader.ReadToEnd());
            return npcJsonObjects;
        }
    }
}
