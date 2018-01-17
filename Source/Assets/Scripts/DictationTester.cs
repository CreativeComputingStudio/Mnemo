using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule.Tests;

public class DictationTester : MonoBehaviour
{

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        GetComponent<DictationRecordButton>().ToggleRecording();
    }
}


