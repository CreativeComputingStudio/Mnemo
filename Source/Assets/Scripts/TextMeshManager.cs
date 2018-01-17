using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshManager : MonoBehaviour {

    public static TextMeshManager instance;
    // Text Mesh Object
    public GameObject textMeshObject;
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

    // Text List
    public List<TextMesh> Meshes;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            // Initialization of the text list
            Meshes = new List<TextMesh>();
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

    private void Update()
    {
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
        Debug.Log("Meshes.Capacity: " + Meshes.Capacity);
    }

    // Update is called once per frame
    public void createText ()
    {
        // Create new text on the gaze position
        GameObject tempText = Instantiate(Resources.Load("Prefab/CursorText"), textMeshObject.transform.position, textMeshObject.transform.rotation) as GameObject;
        tempText.GetComponent<TextMesh>().text = textMeshObject.GetComponent<TextMesh>().text;
        // Set new text face to player
        tempText.transform.rotation = Quaternion.LookRotation(tempText.transform.position - Camera.main.transform.position);
        // Store new text to a list and can be used later
        Meshes.Add(tempText.GetComponent<TextMesh>());
    }

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

        SaveManager.Instance.saveableObjects.Clear();
    }
}
