using System;
using UnityEngine;
using System.IO;

public class DialogReader : MonoBehaviour
{
    [SerializeField] private TextAsset dialogJsonFile;
    [HideInInspector] public Dialog dialogData;
    private OriginalDialog orDia;

    [System.Serializable]
    public class DialogLine
    {
        public CharacterType speaker;
        public Pose pose;
        public Staging stage;
        public string text1;
        public string text2;
        public Staging moveNext;
        public Pose poseNext;
        public CharacterType alsoPose;
        public Pose otherPose;
        public bool imgFlash;
    }

    [System.Serializable]
    public class Dialog
    {
        public DialogLine[] dialog;
    }

    [System.Serializable]
    public class OriginalDialogLine
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
        public bool imgFlash;
    }

    [System.Serializable]
    public class OriginalDialog
    {
        public OriginalDialogLine[] orDiaLine;
    }

    private void Start()
    {
        orDia = JsonUtility.FromJson<OriginalDialog>(dialogJsonFile.text);
        ConvertDialogDataTypes(orDia);
    }

    private void ConvertDialogDataTypes(OriginalDialog dialogToConvert)
    {
        dialogData = new Dialog();
        dialogData.dialog = new DialogLine[dialogToConvert.orDiaLine.Length];
        for (int i = 0; i < dialogToConvert.orDiaLine.Length; i++)
        {
            dialogData.dialog[i] = new DialogLine();
            dialogData.dialog[i].speaker = Enum.TryParse(dialogToConvert.orDiaLine[i].speaker, out CharacterType speaker)
                ? speaker
                : CharacterType.Failed;

            dialogData.dialog[i].pose = Enum.TryParse(dialogToConvert.orDiaLine[i].pose, out Pose pose)
                ? pose
                : Pose.Failed;

            dialogData.dialog[i].stage = Enum.TryParse(dialogToConvert.orDiaLine[i].stage, out Staging stage)
                ? stage
                : Staging.Failed;
            
            dialogData.dialog[i].text1 = dialogToConvert.orDiaLine[i].text1;
            dialogData.dialog[i].text2 = dialogToConvert.orDiaLine[i].text2;
            
            dialogData.dialog[i].moveNext = Enum.TryParse(dialogToConvert.orDiaLine[i].moveNext, out Staging moveNext)
                ? moveNext
                : Staging.Failed;
            
            //dialogData.dialog[i].moveNext = (Staging)Enum.Parse(typeof(Staging), dialogToConvert.orDiaLine[i].moveNext);
            dialogData.dialog[i].poseNext = Enum.TryParse(dialogToConvert.orDiaLine[i].poseNext, out Pose poseNext)
                ? poseNext
                : Pose.Failed;

            dialogData.dialog[i].alsoPose =
                Enum.TryParse(dialogToConvert.orDiaLine[i].alsoPose, out CharacterType alsoPose)
                    ? alsoPose
                    : CharacterType.Failed;


            dialogData.dialog[i].otherPose = Enum.TryParse(dialogToConvert.orDiaLine[i].otherPose, out Pose otherPose)
                ? otherPose
                : Pose.Failed;
            
            dialogData.dialog[i].imgFlash = dialogToConvert.orDiaLine[i].imgFlash;
        }
    }
}
