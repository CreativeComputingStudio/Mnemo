﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType { UserTextMesh }
/**
 * Class for saveable objects
**/
public abstract class SaveableObject : MonoBehaviour {

    protected string saveStats;

    [SerializeField]
    private ObjectType objectType;

	// Use this for initialization
	void Start () {
        SaveManager.Instance.saveableObjects.Add(this);
	}
	
    // method for saving objects
	public virtual void Save (int id, string saveID)
    {   // saveID-id
        PlayerPrefs.SetString(saveID + "-" + id.ToString(), objectType + "_" + transform.localPosition.ToString()
            + "_" + transform.localScale.ToString()
            + "_" + transform.localRotation.ToString()
            + "_" + saveStats);
    }

    // method for loading previous saves
    public virtual void Load (string[] values)
    {
        transform.localPosition = SaveManager.Instance.StringToVector(values[1]);
        transform.localScale = SaveManager.Instance.StringToVector(values[2]);
        transform.localRotation = SaveManager.Instance.StringToQuaternion(values[3]);
    }

    // method for destroying objects
    public void DestroySaveable ()
    {
        SaveManager.Instance.saveableObjects.Remove(this);
        Destroy(gameObject);

    }
}
