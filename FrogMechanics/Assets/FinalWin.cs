﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalWin : MonoBehaviour
{

    public GameObject WinScreen;

    void Update()
    {
       if (PlayerController.totalArtifacts == 3)
        {
            Cursor.lockState = CursorLockMode.None;
            WinScreen.SetActive(true);
        }
    }
}