using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public List<Sprite> CharacterImages;
    [SerializeField] private Image currentImage;
    private CharacterPanel.Staging CurrentStaging = CharacterPanel.Staging.ForegroundCenter;
    
    public void DoCharacterDialog(NPCDialog whichDialog, Sprite characterImage, CharacterPanel.Staging staging)
    {
        currentImage.sprite = characterImage;
        CurrentStaging = staging;
    }
}
