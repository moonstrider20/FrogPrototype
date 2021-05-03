using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextFile : MonoBehaviour
{
    public ActivateTextAtLine activateText;
    public bool duck;
    public bool rat;
    public bool ax;

    string duckPath = "Assets/Talking.txt";
    // Start is called before the first frame update
    void Start()
    {
        if (duck)
            if (PlayerController.strawberryArt)
            {
                activateText.theText = Resources.Load(duckPath) as TextAsset;
            }
    }
}
