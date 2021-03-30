using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public GameObject LoseScreen;
    public GameObject crosshair;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            crosshair.SetActive(false);
            LoseScreen.SetActive(true);
        }
    }

}
