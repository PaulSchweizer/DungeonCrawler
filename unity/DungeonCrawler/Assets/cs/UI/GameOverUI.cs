using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SlotSystem;

public class GameOverUI : MonoBehaviour
{
    // Internals
    public static GameOverUI Instance;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    #region Actions

    #endregion

}
