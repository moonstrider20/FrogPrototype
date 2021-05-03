using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

    public GameObject frog;
    public void Reload()
    {

        

        SceneManager.LoadScene("GodTreeHub");

        frog.transform.position = new Vector3(60.72f, 30.292f, -162.07f);
    }
}