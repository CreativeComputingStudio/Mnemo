using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;

public class TestDictationRecognizer : MonoBehaviour
{
    DictationRecognizer dictationRecognizer;
    /// <summary>
    /// Object attach to text mesh
    /// </summary>
    public TextMesh showDictation;
    /// <summary>
    /// Attach to TextMeshManager.cs
    /// </summary>
    public TextMeshManager newTextMesh;
    /// <summary>
    /// Attach to spatialMapping.cs
    /// </summary>
    public GameObject mappingControl;
    /// <summary>
    /// Attach to game menu
    /// </summary>
    public GameObject menuObject;
    /// <summary>
    /// Attach to developer info menu
    /// </summary>
    public GameObject infoMenu;
    /// <summary>
    /// flag of menu status
    /// </summary>
    public bool isMenuShown = false;
    public bool isInfoShown = false;

    // Use this for initialization
    void Start()
    {
        enableDictation();
    }

    void Update()
    {
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            Debug.LogFormat("Status: " + dictationRecognizer.Status);
        }
    }

    public void enableDictation()
    {
        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += onDictationResult;
        dictationRecognizer.DictationHypothesis += onDictationHypothesis;
        dictationRecognizer.DictationComplete += onDictationComplete;
        dictationRecognizer.DictationError += onDictationError;

        dictationRecognizer.Start();
    }

    void onDictationResult(string text, ConfidenceLevel confidence)
    {
        // write your logic here
        Debug.LogFormat("Confidence: " + confidence+ " Dictation result: " + text);
        showDictation.text = text;

        // add voice commands
        if (string.Equals(text, "disable map"))
        {
            print("Commands: disable map");
            mappingControl.GetComponent<SpatialMapping>().disableMeshRender();
        }
        else if (string.Equals(text, "enable map"))
        {
            print("Commands: enable map");
            mappingControl.GetComponent<SpatialMapping>().enableMeshRender();
        }
        else if (string.Equals(text, "clear text"))
        {
            newTextMesh.clearText();
        }
        else if (string.Equals(text, "menu"))
        {
            // disable other shown info menus
            if (isInfoShown) infoMenu.SetActive(false);

            // enable/disable menu
            if (menuObject.activeSelf)
            {
                menuObject.SetActive(false);
                isMenuShown = false;
            } else
            {
                menuObject.SetActive(true);
                isMenuShown = true;
            }
        }
        else if (string.Equals(text, "Information"))
        {
            // disable other shown info menus
            if (isMenuShown) menuObject.SetActive(false);

            // enable/disable menu
            if (infoMenu.activeSelf)
            {
                infoMenu.SetActive(false);
                isInfoShown = false;
            }
            else
            {
                infoMenu.SetActive(true);
                isInfoShown = true;
            }
        }
        else if (string.Equals(text, "save color red"))
        {
            SaveManager.Instance.Save("saveA");
        }
        else if (string.Equals(text, "save color blue"))
        {
            SaveManager.Instance.Save("saveB");
        }
        else if (string.Equals(text, "save color green"))
        {
            SaveManager.Instance.Save("saveC");
        }
        else if (string.Equals(text, "load color red"))
        {
            SaveManager.Instance.Load("saveA");
        }
        else if (string.Equals(text, "load color blue"))
        {
            SaveManager.Instance.Load("saveB");
        }
        else if (string.Equals(text, "load color green"))
        {
            SaveManager.Instance.Load("saveC");
        }
        else
        {
            newTextMesh.createText();
        }
        showDictation.text = string.Empty;
    }

    void onDictationHypothesis(string text)
    {
        // write your logic here
        Debug.LogFormat("Dictation hypothesis: {0}", text);
        showDictation.text = text+"...";
    }

    void onDictationComplete(DictationCompletionCause cause)
    {
        // write your logic here
        if (cause != DictationCompletionCause.Complete)
            Debug.LogFormat("Dictation completed unsuccessfully: {0}.", cause);
        else
            Debug.LogFormat("Dictation completed successfully");
        Invoke("enableDictation", 1f);
    }

    void onDictationError(string error, int hresult)
    {
        // write your logic here
        Debug.LogFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        Invoke("enableDictation", 1f);
    }
}