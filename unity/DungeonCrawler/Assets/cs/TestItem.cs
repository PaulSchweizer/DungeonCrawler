using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestItem : ScriptableObject
{
    public string Name;
    public TestAspect[] Aspects;
    public string[] Tags;
    public bool IsUnique;

    new public static TestItem DeserializeFromJson(string json)
    {
        return JsonConvert.DeserializeObject<TestItem>(json);
    }
}