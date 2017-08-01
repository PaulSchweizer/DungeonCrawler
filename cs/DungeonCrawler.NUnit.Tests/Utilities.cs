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

    /// <summary>
    /// Mocks the dice roll and provides a predictable result.
    /// </summary>
    internal class NonRandomDie : Random
    {
        public static int DieResult = 0;

        public NonRandomDie(int dieResult = 0)
        {
            DieResult = dieResult;
        }

        public override int Next(int a, int b)
        {
            return DieResult;
        }
    }
}
