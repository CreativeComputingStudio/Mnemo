using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * Class for cursor flashing between text and blank.
**/
public class BlinkingCursor : MonoBehaviour
{
    // time stamp
    private float m_TimeStamp;
    // flag for cursor status
    private bool cursor = false;
    // cursor string
    private string cursorChar = "";
    // length of the text
    private int maxStringLength = 24;

    void Update()
    {
        // setup time gap between each status change
        if (Time.time - m_TimeStamp >= 0.5)
        {
            m_TimeStamp = Time.time;
            if (cursor == false)
            {
                // update cursor status
                // with blank
                cursor = true;
                cursorChar += "_";
            }
            else
            {
                cursor = false;
                if (cursorChar.Length != 0)
                {
                    // update cursor status
                    // with string
                    cursorChar = cursorChar.Substring(0, cursorChar.Length - 1);
                }
            }
        }
    }

    // method returing current cursor strings
    public string getBlinkingCursor ()
    {
        return cursorChar;
    }
}