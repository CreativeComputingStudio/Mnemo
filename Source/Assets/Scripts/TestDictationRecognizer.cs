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
    /// Attach to Save list info menu
    /// </summary>
    public GameObject saveListMenu;
    /// <summary>
    /// Attach to sharing object manager
    /// </summary>
    public GameObject sharingManager;
    /// <summary>
    /// flag of menu status
    /// </summary>
    private bool isMenuShown = false;
    private bool isInfoShown = false;
    private bool isSaveShown = false;

    // Use this for initialization
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

    // Enable dictation recognizer function
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
        // debug text
        Debug.LogFormat("Confidence: " + confidence+ " Dictation result: " + text);
        showDictation.text = text;

        // add voice commands
        // write your logic here
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
        else
        {
            // single/multi player mode create text
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