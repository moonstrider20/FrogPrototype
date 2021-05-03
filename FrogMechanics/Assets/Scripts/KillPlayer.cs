using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    //public VectorValue playerStorage;       //holds the VectorValue asset found in assets, at the moment in scripts

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("This should kill me...");
        if (other.name == "RigidCollider")  //checks if it is the player
        {
            PlayerController.totalLives--;
            PlayerController.hasDied = true;
        }
    }
}
