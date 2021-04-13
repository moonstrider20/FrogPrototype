using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftReset : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.transform.position = new Vector3(60.72f, 30.292f, -162.07f);
            Debug.Log("AYO BITCH");
        }
    }
}
