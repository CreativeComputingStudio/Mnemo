﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
/**
 * Class for updating text color based on user mode
**/
public class TextColorManager : MonoBehaviour {

    // public unique instance
    public static TextColorManager instance;
    // color flag
    public bool isWhite;
    // sharing text color
    public int colorIndex;
    // flag for locking color in sharing mode
    public bool isLocked;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            isWhite = false;
            // random a index number to pick color
            colorIndex = UnityEngine.Random.Range(0,14);
            isLocked = false;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update () {
        // in connection mode
        if (SharingStage.IsInitialized && SharingStage.Instance.IsConnected)
        {
            if(!isLocked)
            {
                colorIndex = SharingStage.Instance.Manager.GetRoomManager().GetCurrentRoom().GetUserCount() - 1;
                isLocked = true;
            }

            if(isWhite)
            {
                this.GetComponent<TextMesh>().color = Color.white;
            } else
            {
                this.GetComponent<TextMesh>().color = Color.red;
            }          
        }
        // in single user mode
        else
        {
            this.GetComponent<TextMesh>().color = Color.cyan;
            isLocked = false;
        }

        /*
        // debug print for user count
        if(SharingStage.Instance.IsConnected)
        {
            print("Num of room users: " +
                SharingStage.Instance.Manager.GetRoomManager().GetCurrentRoom().GetUserCount()
            );
        }
        */
    }

}
