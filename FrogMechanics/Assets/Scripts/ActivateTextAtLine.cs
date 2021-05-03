/*Amorina Tabera
    This code should be placed on something with a collider, whether that is an NPC so the player
        can talk to them, or a shout zone that when entered an NPC automatically starts talking to the
        player. The colliders should have the "is trigger" box checked in order for this code to work. 
    If you want the player to talk to a character (and not kill that NPC) check "Button Press". Note
        this collider should be separate from the actual NPC's collider unless you want your character 
        to walk through your NPC. 
    If you want something to be destroyed once used (like a shout zone) check "Destroy When Activated"
    Keep in mind I keep saying "talking" but this could work with objects too, like signs to read
        and such. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour
{
    public TextAsset theText;           //Hold .txt from the assets

    public int startLine;               //Where to start in a given .txt (remeber the first line is line 0)
    public int endLine;                 //Where to end in a given .txt

    public TextBox theTextBox;          //The text box gameObject from the hierarchy, simply an empty object with TextBox.cs

    public bool ButtonPress;            //To check if player needs to pressed a button
    private bool waitForPress;          //To wait for player to press button to initiate talking

    public bool destroyWhenActivated;   //To check if the object should be destroyed once used 

    // Update is called once per frame
    void Update()
    {
        //Check if object is allowing player to press a key and if the key is pressed
        //Can change what button to press to initaite talking here, right now it is E
        if (waitForPress && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 2")))
        {
            theTextBox.ReloadScript(theText);       //loads the appropriate .txt file
            theTextBox.currentLine = startLine;     //assigns the starting line
            theTextBox.endAtLine = endLine;         //assigns the ending line
            theTextBox.EnableTextBox();             //Activates the method EnableTextBox form TextBox.cs

            //Checks if this object is meant to be destoryed after the talking is done.
            if (destroyWhenActivated)
            {
                Destroy(gameObject);                //Destroys the gameObject this script is attached to
            }
        }
    }

    //Check if Player has entered talking zone
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Someting touched me!");
        Debug.Log(other.name);
        if (other.name == "RigidCollider")      //Checks if it is the player
        {
            if (ButtonPress)                    //Checks if this object needs a button press to initiate dialogue
            {
                waitForPress = true;            //Yes, so will now wait for that button press
                return;
            }

            Debug.Log("I should be talking");
            theTextBox.ReloadScript(theText);   //loads the appropriate .txt file 
            theTextBox.currentLine = startLine; //assigns the starting line
            theTextBox.endAtLine = endLine;     //assigns the ending line
            theTextBox.EnableTextBox();         //Activates the method EnableTextBox form TextBox.cs

            //Checks if this object is meant to be destoryed after the talking is done.
            if (destroyWhenActivated)
            {
                Destroy(gameObject);            //Destroys the gameObject this script is attached to
            }
        }
    }

    //Checks if player is out of range, mostly for something the character to interact with again
    void OnTriggerExit(Collider other)
    {
        if (other.name == "RigidCollider")      //checks if it is the player
        {
            waitForPress = false;               //no more waiting for button press, out of range
        }
    }
}