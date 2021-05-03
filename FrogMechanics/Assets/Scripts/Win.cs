using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{

    public GameObject WinScreen;
    public GameObject crosshair;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            crosshair.SetActive(false);
            WinScreen.SetActive(true);
        }
    }



 



}
