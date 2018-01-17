using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlinkingCursor : MonoBehaviour
{
    private float m_TimeStamp;
    private bool cursor = false;
    private string cursorChar = "";
    private int maxStringLength = 24;

    void Update()
    {
        if (Time.time - m_TimeStamp >= 0.5)
        {
            m_TimeStamp = Time.time;
            if (cursor == false)
            {
                cursor = true;
                cursorChar += "_";
            }
            else
            {
                cursor = false;
                if (cursorChar.Length != 0)
                {
                    cursorChar = cursorChar.Substring(0, cursorChar.Length - 1);
                }
            }
        }
    }

    public string getBlinkingCursor ()
    {
        return cursorChar;
    }
}