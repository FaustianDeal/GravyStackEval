using UnityEngine;
using System.IO;

public class DialogReader : MonoBehaviour
{
    [SerializeField] private TextAsset dialogJsonFile;
    [HideInInspector] public Dialog dialogData;

    [System.Serializable]
    public class DialogLine
    {
        public string speaker;
        public string pose;
        public string stage;
        public string text1;
        public string text2;
        public string moveNext;
        public string poseNext;
        public string alsoPose;
        public string otherPose;
    }

    [System.Serializable]
    public class Dialog
    {
        public DialogLine[] dialog;
    }

    private void Start()
    {
        dialogData = JsonUtility.FromJson<Dialog>(dialogJsonFile.text);
    }
}
