using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictationLooper : MonoBehaviour {

    public GameObject dictationManager;
    private bool isActive = true;

    public HoloToolkit.Unity.InputModule.CustomDictationManager customDictation;

    private void Start()
    {
        customDictation = dictationManager.GetComponent<HoloToolkit.Unity.InputModule.CustomDictationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("s"))
        {
            if (isActive)
            {
                
                isActive = false;
            }
            else
            {
                dictationManager.SetActive(true);
                isActive = true;
            }

        }
    }
}
