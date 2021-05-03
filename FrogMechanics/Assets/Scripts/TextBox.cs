using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public GameObject textBox;

    public Text theText;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public PlayerController player;     //keep player from moving

    public bool isActive;

    //public bool stopPlayerMovement;

    private bool isTyping = false;
    private bool cancelTyping = false;

    public float typeSpeed;

    [Header("Character Images")]
    public Image rat;
    public Image ax;
    public Image duck;
    //public Image unknown;
    public Image frog;


    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<PlayerController>();

        if (textFile != null)   //check if file there first
        {
            textLines = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
        {
            EnableTextBox();
        }

        else
        {
            DisableTextBox();
        }
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }

        //theText.text = textLines[currentLine];

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isTyping)
            {
                currentLine += 1;

                if (currentLine > endAtLine)
                {
                    DisableTextBox();
                }

                else
                {
                    StartCoroutine(TextScroll(textLines[currentLine]));
                }
            }

            else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    private IEnumerator TextScroll(string lineOfText)  //used for corutines, which work in their own timelines
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        char character;

        character = lineOfText[letter];

        if (character == 'F')
            frog.enabled = true;
        else frog.enabled = false;

        if (character == 'A')
            ax.enabled = true;
        else ax.enabled = false;

        if (character == 'D')
            duck.enabled = true;
        else duck.enabled = false;

        /*if (character == 'U')
            unknown.enabled = true;
        else unknown.enabled = false;*/

        if (character == 'R')
            rat.enabled = true;
        else rat.enabled = false;

        Debug.Log(character);

        letter += 3;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);     //really only for courtines
        }
        theText.text = lineOfText.Remove(0, 3);  //prints all of line if player tries to skip dialouge  
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;

        //if(stopPlayerMovement)
        //{
        player.canMove = false;
        //}
        StartCoroutine(TextScroll(textLines[currentLine]));
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;

        player.canMove = true;
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}