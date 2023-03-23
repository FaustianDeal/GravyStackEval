using UnityEngine;
using System.IO;

public class DialogReader : MonoBehaviour
{
    [SerializeField] private TextAsset dialogJsonFile;

    [System.Serializable]
    private class DialogLine
    {
        public string speaker;
        public string text;
    }

    [System.Serializable]
    private class Dialog
    {
        public DialogLine[] dialog;
    }

    private void Start()
    {
        Dialog dialogData = JsonUtility.FromJson<Dialog>(dialogJsonFile.text);

        foreach (DialogLine dialogLine in dialogData.dialog)
        {
            Debug.Log(dialogLine.speaker + ": " + dialogLine.text);
        }
    }
}
