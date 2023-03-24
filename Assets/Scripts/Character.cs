using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class Character : MonoBehaviour
{
    public CharacterType myCharacterType;
    [SerializeField] private NPCDialog myDialog;
    private List<Sprite> CharacterSprites;
    [SerializeField] private Image currentImage;
    public ImageFader myImageFader;
    private Staging CurrentStaging = Staging.ForegroundCenter;
    private readonly string folderPath = "Assets/Game Dev Test Assets";

    public void Initialize()
    {
        string[] files = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);
        CharacterSprites = new List<Sprite>();
        foreach (string file in files)
        {
            if (file.Contains(myCharacterType.ToString())) // use myCharacterType directly
            {
                Sprite image = AssetDatabase.LoadAssetAtPath<Sprite>(file);
                if (image != null)
                {
                    CharacterSprites.Add(image);
                }
            }
        }
    }
    public void DoCharacterDialog(Pose whatPose, Staging staging, bool hasTwoParts, string dialog1, string dialog2 = null)
    {
        SetPose(whatPose);
        
        CurrentStaging = staging;
        myDialog.gameObject.SetActive(true);
        myDialog.DoDialog(hasTwoParts,dialog1,staging,dialog2);
    }

    public void DisableDialog()
    {
        myDialog.gameObject.SetActive(false);
    }

    public void SetPose(Pose newPose)
    {
        for (int i = 0; i < CharacterSprites.Count; i++)
        {
            if (CharacterSprites[i].name.Contains(newPose.ToString()))
            {
                currentImage.sprite = CharacterSprites[i];
                currentImage.SetNativeSize();
                //currentImage.rectTransform.sizeDelta = new Vector2(width, height);
            }
        }
        myImageFader.FadeIn();
    }
}
