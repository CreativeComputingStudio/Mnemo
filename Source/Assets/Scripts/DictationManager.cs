using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;

public class DictationManager : MonoBehaviour
{

    DictationRecognizer dictationRecognizer;


    // Use this for initialization
    void Start()
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
        Debug.LogFormat("Dictation result: " + text);
    }

    void onDictationHypothesis(string text)
    {
        // write your logic here
        Debug.LogFormat("Dictation hypothesis: {0}", text);
    }

    void onDictationComplete(DictationCompletionCause cause)
    {
        // write your logic here
        if (cause != DictationCompletionCause.Complete)
            Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", cause);
    }

    void onDictationError(string error, int hresult)
    {
        // write your logic here
        Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
    }
}