using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Class for specific saveable objects
**/
public class SpecificObject : SaveableObject {

    // Deleted Start () function in case it is not override

    public override void Save(int id, string saveID)
    {
        // save Mesh text
        //Debug.Log("save: " + GetComponent<TextMesh>().text);
        saveStats = GetComponent<TextMesh>().text;
        base.Save(id, saveID);
    }

    public override void Load(string[] values)
    {
        // load Mesh text
        GetComponent<TextMesh>().text = values[4];
        TextMeshManager.instance.Meshes.Add(GetComponent<TextMesh>());
        base.Load(values);
    }
}
