using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WorldmapController : MonoBehaviour
{
    public void SwitchLocation(string destination)
    {
        MainController.SwitchLocation(destination);
    }
}
