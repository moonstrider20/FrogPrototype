/*Amorina Tabera
    This code needs to be placed on something with a collider. The "is trigger" box should be checked
    For now this is hard coding. You will need to know the location (x, y, z) you want the player to 
        spawn at in the scene they are about to travel to. These coordinates will be placed on the object
        in the inspector window. 
    You will also need to know the index of the scene. This can be found in build settings. When you
        add a scene, it gets assigned an index on the right side. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public int scene;                       //the index of the scene player will travel to
    public Vector3 playerPosition;          //position you want the player to be in the next scene
    public VectorValue playerStorage;       //holds the VectorValue asset found in assets, at the moment in scripts

    [Header("Use for artifacts")]
    public bool artifactFound;              //Check box in the inspector if this is attached to an artifact or a portal that opens after artifact is aquired
    public int artifactNumber;              //Enter what number artifact this is

    [Header("Use for normal portals")]
    public bool requiresArtifact;           //Check box if an artifact is required to use
    public int artifactTotal;               //Enter number of artifacts required to enter

    //Checks if something entered the "portal"
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "RigidCollider")  //checks if it is the player
        {
            //If it is a portal and requires a certain number of artifacts
            if (requiresArtifact)
            {
                //Checks if player is allowed to enter
                //If not enough, exit script
                if (PlayerController.totalArtifacts < artifactTotal)
                    return;
            }

            //If it is an artifact that you need to collect
            if (artifactFound)
            {
                //Checks if the artifact has been collected already
                if (artifactNumber > PlayerController.totalArtifacts)
                {
                    //If not, increment the totalArtifacts
                    PlayerController.totalArtifacts++;
                }
            }
            playerStorage.initalValue = playerPosition; //sets the player's position for the next scene
            SceneManager.LoadScene(scene);              //loads the new scene
        }
    }
}