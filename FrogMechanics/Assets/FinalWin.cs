using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalWin : MonoBehaviour
{

    public GameObject WinScreen;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "RigidCollider")      //Checks if it is the player
            if (PlayerController.totalArtifacts == 3)
            {
                Cursor.lockState = CursorLockMode.None;
                WinScreen.SetActive(true);
            }
    }
}
