using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ExitUI : MonoBehaviour
{
    public Text DestinationText;
    public Button OkButton;
    public Button CancelButton;

    private string Destination;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show(string destination)
    {
        gameObject.SetActive(true);
        Destination = destination;
        DestinationText.text = string.Format("Do you want to travel to {0}?", destination.Split('.')[0]);
    }

    public void SwitchLocation()
    {
        MainController.SwitchLocation(Destination);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

