using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestingSerialization : MonoBehaviour
{
    public void Start()
    {
        string json = "{\"Name\": \"Hello\", \"Aspects\": [{\"Name\": \"Hello Iam an Aspect\"}]}";
        TestItem item = TestItem.DeserializeFromJson(json);

        Debug.Log(item.Aspects[0].Name);
    }
}

