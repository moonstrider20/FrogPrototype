using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerController.totalArtifacts >= 3)
        {
            Debug.Log("DeStRoY");
            Destroy(gameObject);
        }
    }
}
