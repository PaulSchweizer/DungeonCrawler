using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DungeonCrawler.NUnit.Tests
{
    public static class Utilities
    {
        public static string JsonResource(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Format("DungeonCrawler.NUnit.Tests.Resources.{0}.json", file);
            string json;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }
    }
}
