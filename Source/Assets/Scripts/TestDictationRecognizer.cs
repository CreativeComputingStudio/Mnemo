using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;

/**
 * Class for dictation recognizer
 * Handling all voice commands
**/
public class TestDictationRecognizer : MonoBehaviour
{
    // dictation recognizer from holotool-kit
    DictationRecognizer dictationRecognizer;
    // object attached to text mesh
    public TextMesh showDictation;
    // object attached to TextMeshManager.cs
    public TextMeshManager newTextMesh;
    // object attached to spatialMapping.cs
    public GameObject mappingControl;
    // object attached to game menu
    public GameObject menuObject;
    // object attached to developer info menu
    public GameObject infoMenu;
    // object attached to Save list info menu
    public GameObject saveListMenu;
    // object attached to connection menu
    public GameObject connectionMenu;
    // flag of menu status
    private bool isMenuShown = false;
    private bool isInfoShown = false;
    private bool isSaveShown = false;

    // use this for initialization
    void Start()
    {
        if(menuObject.activeSelf)
        {
            isMenuShown = true;
        }
        enableDictation();
    }

    void Update()
    {
        // showing speech status
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            Debug.LogFormat("Status: " + dictationRecognizer.Status);
        }
    }

    // enable dictation recognizer function
    public void enableDictation()
    {
        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += onDictationResult;
        dictationRecognizer.DictationHypothesis += onDictationHypothesis;
        dictationRecognizer.DictationComplete += onDictationComplete;
        dictationRecognizer.DictationError += onDictationError;

        dictationRecognizer.Start();
    }

    // method for voice commands triggering
    void onDictationResult(string text, ConfidenceLevel confidence)
    {
        // debug text
        Debug.LogFormat("Confidence: " + confidence+ " Dictation result: " + text);
        showDictation.text = text;

        // add voice commands
        // write your logic here
        // command: disable map - disable mesh render 
        if (string.Equals(text, "disable map"))
        {
            print("Commands: disable map");
            mappingControl.GetComponent<SpatialMapping>().disableMeshRender();
        }
        // command: enable map - enable mesh render
        else if (string.Equals(text, "enable map"))
        {
            print("Commands: enable map");
            mappingControl.GetComponent<SpatialMapping>().enableMeshRender();
        }
        // command: clear all - clear all text
        else if (string.Equals(text, "clear all"))
        {
            newTextMesh.clearText();
        }
        // command: menu - enable or disable main menu
        else if (string.Equals(text, "menu"))
        {
            // disable other shown info menus
            if (isInfoShown) infoMenu.SetActive(false);
            isInfoShown = false;
            if (isSaveShown) saveListMenu.SetActive(false);
            isSaveShown = false;

            // enable/disable menu
            if (menuObject.activeSelf)
            {
                menuObject.SetActive(false);
                isMenuShown = false;
            }
            else
            {
                menuObject.SetActive(true);
                isMenuShown = true;
            }
        }
        // command: information - enable or disable information menu
        else if (string.Equals(text, "information"))
        {
            // disable other shown info menus
            if (isMenuShown) menuObject.SetActive(false);
            isMenuShown = false;
            if (isSaveShown) saveListMenu.SetActive(false);
            isSaveShown = false;

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
        // command: connection - enable or disable connection menu
        else if (string.Equals(text, "connection"))
        {
            if (connectionMenu.activeSelf)
            {
                connectionMenu.SetActive(false);
            } else
            {
                connectionMenu.SetActive(true);
            }
        }
        // command: save - save all text
        else if (text.Contains("save"))
        {
            // initialization
            string[] tmpStrs = text.Split(' ');
            int tmpSaveID = -1;
            string tmpSaveStr = "";
            // find save ID string
            for (int tmpIndex = 0; tmpIndex < tmpStrs.Length; tmpIndex++)
            {
                if (tmpSaveID == -1)
                {
                    if (string.Equals(tmpStrs[tmpIndex], "save"))
                    {
                        if (tmpIndex < tmpStrs.Length - 1)
                        {
                            tmpSaveID = tmpIndex;
                        }
                        continue;
                    }
                }
                else if (tmpSaveID != -1)
                {
                    tmpSaveStr += tmpStrs[tmpIndex] + " ";
                }
            }

            // remove last space
            tmpSaveStr = tmpSaveStr.Remove(tmpSaveStr.Length - 1);
            // call save
            SaveManager.Instance.Save(tmpSaveStr);
            print(tmpSaveStr);
        }
        // command: load - load text based on saved name
        else if (text.Contains("load"))
        {
            // initialization
            string[] tmpStrs = text.Split(' ');
            int tmpLoadID = -1;
            string tmpLoadStr = "";
            // find load ID string
            for (int tmpIndex = 0; tmpIndex < tmpStrs.Length; tmpIndex++)
            {
                if (tmpLoadID == -1)
                {
                    if (string.Equals(tmpStrs[tmpIndex], "load"))
                    {
                        if (tmpIndex < tmpStrs.Length - 1)
                        {
                            tmpLoadID = tmpIndex;
                        }
                        continue;
                    }
                }
                else if (tmpLoadID != -1)
                {
                    tmpLoadStr += tmpStrs[tmpIndex] + " ";
                }
            }

            // remove last space
            tmpLoadStr = tmpLoadStr.Remove(tmpLoadStr.Length - 1);
            // call load
            SaveManager.Instance.Load(tmpLoadStr);
            print(tmpLoadStr);
        }
        // command: browse - show all saved files' name
        else if (string.Equals(text, "browse"))
        {
            // ONLY USE FOR TESTING SAVE LIST
            TextMesh currentText = saveListMenu.GetComponent<TextMesh>();
            currentText.text = " - Browse List - \n";
            //print("Saved List size: " + SaveManager.Instance.saveList.Count);
            if (SaveManager.Instance.saveList.Count < 1)
            {
                currentText.text += " Empty \n";
            }
            else
            {
                for (int index = 0; index < SaveManager.Instance.saveList.Count; index++)
                {
                    currentText.text += SaveManager.Instance.saveList[index] + "\n";
                    //print(index + " : " + SaveManager.Instance.saveList[index]);
                }
            }

            // disable other shown info menus
            if (isMenuShown) menuObject.SetActive(false);
            isMenuShown = false;
            if (isInfoShown) infoMenu.SetActive(false);
            isInfoShown = false;

            // enable/disable save menu
            if (saveListMenu.activeSelf)
            {
                saveListMenu.SetActive(false);
                isSaveShown = false;
            }
            else
            {
                saveListMenu.SetActive(true);
                isSaveShown = true;
            }
        }
        // command: quit - exit the application
        else if (string.Equals(text, "quit"))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        // end of voice commands
        else
        {
            // single/multi player mode create text
            newTextMesh.createText();
        }
        showDictation.text = string.Empty;
    }

    // method for dication manager hypothesis results
    void onDictationHypothesis(string text)
    {
        // print hypothesis results
        Debug.LogFormat("Dictation hypothesis: {0}", text);
        showDictation.text = text+"...";
    }

    // method for dictation manager complete results
    void onDictationComplete(DictationCompletionCause cause)
    {
        // return dictation result when complete status
        if (cause != DictationCompletionCause.Complete)
            Debug.LogFormat("Dictation completed unsuccessfully: {0}.", cause);
        else
            Debug.LogFormat("Dictation completed successfully");
        Invoke("enableDictation", 1f);
    }

    // method for dictation manager error results
    void onDictationError(string error, int hresult)
    {
        // report error when flag
        Debug.LogFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        Invoke("enableDictation", 1f);
    }
}