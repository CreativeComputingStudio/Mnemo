using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Class for saving game objects in current scene.
**/
public class SaveManager : MonoBehaviour {

    // save manager instance for public call
    private static SaveManager instance;
    // list for all saved objects' name
    public List<string> saveList;
    // list for all saved game objects
    public List<SaveableObject> saveableObjects { get; private set; }

    // instance initializtion
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SaveManager>();
            }
            return instance;
        }
    }

	// use this for awake phase initialization
	void Awake () {
        saveableObjects = new List<SaveableObject> ();
        saveList = new List<string>();
	}

    // main Save method based on voice ID
    public void Save (string saveID)
    {
        // save total number of objects saved
        PlayerPrefs.SetInt(saveID, saveableObjects.Count);
        saveList.Add(saveID);

        // loop the list and save all objects
        for (int indexOfList = 0; indexOfList < saveableObjects.Count; indexOfList++)
        {
            saveableObjects[indexOfList].Save(indexOfList, saveID);
        }
    }

    // main Load method based on voice ID
    public void Load (string saveID)
    {
        // erase old objects before loading
        foreach (SaveableObject obj in saveableObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
        // clear saved lists
        saveableObjects.Clear();
        TextMeshManager.instance.clearText();

        int objectCount = PlayerPrefs.GetInt(saveID);

        // load saved value of objects
        for (int indexOfList = 0; indexOfList < objectCount; indexOfList++)
        {
            string[] value = PlayerPrefs.GetString(saveID + "-" + indexOfList.ToString()).Split('_');
            // value[0] contains object type
            // value[1] contains saved properties
            // using switch case here for future loading different type of prefabs
            GameObject tmpObj = null;
            switch (value[0])
            {
                case "UserTextMesh":
                    tmpObj = Instantiate(Resources.Load("Prefab/CursorText") as GameObject);
                    break;
            }
            
            if (tmpObj != null)
            {
                tmpObj.GetComponent<SaveableObject>().Load(value);
            }
        }  
    }

    // function for converting strings
    public Vector3 StringToVector (string value)
    {
        try
        {
            // (1, 2,3);
            value = value.Trim(new char[] { '(', ')' });
            // 1, 2,3
            value = value.Replace(" ", "");
            // 1,2,3
            string[] pos = value.Split(',');
            // [0] = 1, [1] = 2, [2] = 3
            return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
        } catch (System.Exception e)
        {
            Debug.Log("StringToVector Error");
            return Vector3.zero;
        }
    }

    // function for converting strings
    public Quaternion StringToQuaternion (string value)
    {
        try
        {
            // (1, 2,3);
            value = value.Trim(new char[] { '(', ')' });
            // 1, 2,3
            value = value.Replace(" ", "");
            // 1,2,3
            string[] pos = value.Split(',');
            // [0] = 1, [1] = 2, [2] = 3
            return new Quaternion(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]), float.Parse(pos[3]));
        }
        catch (System.Exception e)
        {
            Debug.Log("StringToVector Error");
            return Quaternion.identity;
        }
    }
}
