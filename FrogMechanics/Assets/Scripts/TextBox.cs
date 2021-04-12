﻿using System.Collections;
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

        if (Input.GetKeyDown(KeyCode.Return))
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

        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);     //really only for courtines
        }
        theText.text = lineOfText;
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