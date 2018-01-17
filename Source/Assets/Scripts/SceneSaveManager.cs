using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSaveManager : MonoBehaviour {

    public static SceneSaveManager instance;

    public GameObject TextMeshManagerObject;

    private TextMeshManager textMeshManagerScript;

	// Use this for initialization
	void Start () {
        if (instance == null)
        {
            instance = this;
            // load current text mesh manager script
            //textMeshManagerScript = TextMeshManagerObject.GetComponent<TextMeshManager>();

        } else if (instance != null)
        {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void saveObjectAnchors ()
    {
        for (int indexOfList = 0; indexOfList < textMeshManagerScript.Meshes.Count; ++indexOfList)
        {
            
        }
    }
}
