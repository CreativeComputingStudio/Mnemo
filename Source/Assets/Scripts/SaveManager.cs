using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    private static SaveManager instance;

    public List<SaveableObject> saveableObjects { get; private set; }

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


	// Use this for initialization
	void Awake () {
        saveableObjects = new List<SaveableObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Save (string saveID)
    {
        // save total number of objects saved
        PlayerPrefs.SetInt(saveID, saveableObjects.Count);

        // loop the list and save all objects
        for (int indexOfList = 0; indexOfList < saveableObjects.Count; indexOfList++)
        {
            saveableObjects[indexOfList].Save(indexOfList, saveID);
        }
    }

    public void Load (string saveID)
    {
        // Erase old objects before loading
        foreach (SaveableObject obj in saveableObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
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
