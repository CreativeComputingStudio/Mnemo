using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * Class for cursor blinking
**/
public class CursorManager : MonoBehaviour
{
    // object attached to cursor text mesh
    public TextMesh cursorText;

    // time gap between blink
    private float m_TimeStamp;
    // cursor status
    private bool cursor = false;
    // cursor strings
    private string cursorChar = "";

    // Update is called once per frame
    void Update () {
		if (cursorText.text == string.Empty || string.Equals(cursorText.text, "|"))
        {
            // set color of text
            TextColorManager.instance.isWhite = false;
            if (Time.time - m_TimeStamp >= 0.5)
            {
                m_TimeStamp = Time.time;
                if (cursor == false)
                {
                    // update cursor with blank and I
                    cursor = true;
                    cursorChar = "|";
                }
                else
                {
                    // update cursor with stirng.empty
                    cursor = false;
                    cursorChar = "";
                }
            }
            cursorText.text = cursorChar;
        }
	}
}
