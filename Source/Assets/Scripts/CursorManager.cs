using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public TextMesh cursorText;

    private float m_TimeStamp;
    private bool cursor = false;
    private string cursorChar = "";
    private int maxStringLength = 24;

    // Update is called once per frame
    void Update () {
		if (cursorText.text == string.Empty || string.Equals(cursorText.text, "|"))
        {
            if (Time.time - m_TimeStamp >= 0.5)
            {
                m_TimeStamp = Time.time;
                if (cursor == false)
                {
                    cursor = true;
                    cursorChar = "|";
                }
                else
                {
                    cursor = false;
                    cursorChar = "";
                }
            }
            cursorText.text = cursorChar;
        }
	}
}
