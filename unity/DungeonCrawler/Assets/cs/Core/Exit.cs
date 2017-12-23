using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public string Destination;

    private void OnTriggerEnter(Collider other)
    {
        MainController.SwitchLocation(Destination);
    }
}
