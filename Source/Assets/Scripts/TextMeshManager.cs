using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
/**
 * Class for managing text mesh in scene.
 * 
**/
public class TextMeshManager : MonoBehaviour {

    // public text mesh mananger instance
    public static TextMeshManager instance;
    // Text Mesh Object
    public GameObject textMeshObject;
    // attach to sharing spawn manager
    public PrefabSpawnManager spawnManager;
    // Fade levels (255, 100 or 10)
    public enum fadeLevel
    {
        Large,
        Mid,
        Small
    }
    // Selected gray fade level for text
    public fadeLevel GrayLevel;
    // Total fade value based on fade level
    private float totalFadeValue;
    // Sync object list
    public List<SyncSpawnedObject> SyncObjectList;

    // Text List
    public List<TextMesh> Meshes;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            // Initialization of the text list
            Meshes = new List<TextMesh>();
            SyncObjectList = new List<SyncSpawnedObject>();
            Meshes.Capacity = 256;
            // Setup total fade value based on selected fade level
            if (GrayLevel == fadeLevel.Large) totalFadeValue = 255f;
            else if (GrayLevel == fadeLevel.Mid) totalFadeValue = 100f;
            else if (GrayLevel == fadeLevel.Small) totalFadeValue = 10f;
        } else if (instance != null)
        {
            Destroy(this);
        }
    }

    // currently disabled
    private void Update()
    {
        // Text fade based on Time
        /*
        for (int indexOfList = 0; indexOfList < Meshes.Count; indexOfList++)
        {
            // Set all text always face to the player
            //AllTextMesh[indexOfList].transform.rotation = Quaternion.LookRotation(AllTextMesh[indexOfList].transform.position - Camera.main.transform.position);
            // Create new color
            float fadeColorValue = totalFadeValue - Meshes.Count + indexOfList;
            fadeColorValue = fadeColorValue / totalFadeValue;

            if (fadeColorValue < 0.1f)
            {
                fadeColorValue = 0.1f;
            }
            Color fadeColor = new Color(fadeColorValue, fadeColorValue, 1, 1);
            Meshes[indexOfList].color= fadeColor;
        }
        */
        //Debug.Log("Meshes.Capacity: " + Meshes.Capacity);
    }

    // Update is called once per frame
    public void createText ()
    {
        // multiplayer text creation mode
        if(SharingStage.IsInitialized && SharingStage.Instance.IsConnected)
        {
            // set color
            TextColorManager.instance.isWhite = true;
            // using sharing service to create new texts
            Quaternion faceToCreator = Quaternion.LookRotation(textMeshObject.transform.position - Camera.main.transform.position);
            SyncSpawnedObject tmpSynObj = new SyncSpawnedObject();
            // call spawn manager to spawn sync object
            this.spawnManager.Spawn(
                tmpSynObj,
                textMeshObject.transform.position,
                faceToCreator,
                null,
                "SynObject",
                false,
                textMeshObject.GetComponent<TextMesh>().text);
            // update spawned object text
            tmpSynObj.GameObject.GetComponent<TextMesh>().text = textMeshObject.GetComponent<TextMesh>().text;
            //tmpSynObj.GameObject.GetComponent<TextMesh>().color = Color.white;
            // update list
            SyncObjectList.Add(tmpSynObj);
        } else
        {
            // local text creation mode
            // Create new text on the gaze position
            GameObject tempText = Instantiate(Resources.Load("Prefab/CursorText"), textMeshObject.transform.position, textMeshObject.transform.rotation) as GameObject;
            tempText.GetComponent<TextMesh>().text = textMeshObject.GetComponent<TextMesh>().text;
            // set single mode color to blue
            tempText.GetComponent<TextMesh>().color = Color.white;
            // Set new text face to player
            tempText.transform.rotation = Quaternion.LookRotation(tempText.transform.position - Camera.main.transform.position);
            // Store new text to a list and can be used later
            Meshes.Add(tempText.GetComponent<TextMesh>());
        }
    }

    // method for clear all texts in scene
    public void clearText ()
    {
        for (int indexOfList = Meshes.Count - 1; indexOfList >= 0; indexOfList--)
        {
            // assign temp pointer to the Text object
            GameObject destroyObj = Meshes[indexOfList].gameObject;
            // remove Text object from the list
            Meshes.RemoveAt(indexOfList);
            // delete Text object in the world
            Destroy(destroyObj);
        }

        for (int indexOfSyncList = SyncObjectList.Count - 1; indexOfSyncList >= 0; indexOfSyncList--)
        {
            // assign temp pointer to the Text object
            SyncSpawnedObject destroySyncObj = SyncObjectList[indexOfSyncList];
            if (destroySyncObj != null)
            {
                // remove Text object from the list
                SyncObjectList.RemoveAt(indexOfSyncList);
                // delete Text object in the world
                spawnManager.Delete(destroySyncObj);
            }
        }

        // clear savable object list
        SaveManager.Instance.saveableObjects.Clear();
    }
}
