using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

    public enum CharacterType   //What kind of character
    {
        Failed = -1,
        BronzeBear,
        GreenGecko,
        PurplePenguin
    }
    public enum Pose    // What pose the character should use
    {
        Failed = -1,
        Pose1,
        Pose2,
        Pose3
    }
    public enum Staging // Where a character will appear in the scene
    {
        Failed = -1,
        ForegroundLeft,
        ForegroundLeft2,
        ForegroundCenter,
        ForegroundRight,
        BackgroundLeft,
        BackgroundCenter,
        BackgroundRight
    }
public class CharacterPanel : MonoBehaviour
{
    public GameObject characterPrefab; // The character prefab that you want to place
    public DialogReader dialogReader;
    public ImageFlasher flasher;
    public LetterboxEffect letterBoxEffect;
    public float backgroundStagingScale = .75f;
    private Dictionary<Staging, Transform> characterPanelTransforms;
    private List<Character> SpawnedCharacters;
    private int dialogProgressCounter = -1;
    private CharacterType[] allCharacterTypes;
    private Pose[] allPoses;
    private Staging[] allStages;
    private CharacterType lastActiveCharacter;
    
    void Start()
    {
        characterPanelTransforms = new Dictionary<Staging, Transform>();
        SpawnedCharacters = new List<Character>();
        List<Transform> characterForegroundPositions = new List<Transform>();
        List<Transform> characterBackgroundPositions = new List<Transform>();
        allCharacterTypes = (CharacterType[])Enum.GetValues(typeof(CharacterType));
        allPoses = (Pose[])Enum.GetValues(typeof(Pose));
        allStages = (Staging[])Enum.GetValues(typeof(Staging));
        
        // Loop through the children of this object and add any transforms that have the "Foreground" or "Background" tag
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform childTransform = this.transform.GetChild(i);
            if (childTransform.name.StartsWith("Foreground"))
            {
                characterForegroundPositions.Add(childTransform);
            }
            else if (childTransform.name.StartsWith("Background"))
            {
                characterBackgroundPositions.Add(childTransform);
            }
        }
        
        // Loop through the Staging enum values and the characterPositions arrays
        //Staging[] stagings = (Staging[])Enum.GetValues(typeof(Staging));
        for (int i = 0; i < allStages.Length; i++)
        {
            Transform tfm = null;
    
            if (i < characterForegroundPositions.Count && allStages[i] >= Staging.ForegroundLeft && allStages[i] <= Staging.ForegroundRight)
            {
                tfm = characterForegroundPositions[i];
            }
            else if (i < characterForegroundPositions.Count + characterBackgroundPositions.Count && allStages[i] >= Staging.BackgroundLeft && allStages[i] <= Staging.BackgroundRight)
            {
                tfm = characterBackgroundPositions[i - characterForegroundPositions.Count];
            }

            characterPanelTransforms.Add(allStages[i], tfm);
        }
        //DoCharacterDialog(CharacterType.GreenGecko, Pose.Pose1, Staging.ForegroundCenter, true, "Testing Part 1", "Testing Part 2");
    }
    
    private void OnClick()
    {
        AdvanceTheDialog();
    }

    private void OnEnable()
    {
        TouchOrClickHandler.OnClickEvent += OnClick;
        flasher.ImageFlashComplete += OnImageFlashComplete;
    }

    private void OnDisable()
    {
        TouchOrClickHandler.OnClickEvent -= OnClick;
        flasher.ImageFlashComplete -= OnImageFlashComplete;
    }

    private void OnImageFlashComplete()
    {
        //AdvanceTheDialog();
    }

    private void AdvanceTheDialog()
    {
        if (dialogProgressCounter >= dialogReader.dialogData.dialog.Length-1) return;
        letterBoxEffect.ShowLetterbox();
        if (dialogProgressCounter > -1)
        {
            if (dialogReader.dialogData.dialog[dialogProgressCounter].moveNext != Staging.Failed)
            {
                if (lastActiveCharacter != CharacterType.Failed)
                {
                    foreach (Staging aStage in allStages)
                    {
                        if (aStage == dialogReader.dialogData.dialog[dialogProgressCounter].moveNext)
                        {
                            MoveCharacter(lastActiveCharacter,
                                aStage);
                        }
                    }
                }
            }

            if (dialogReader.dialogData.dialog[dialogProgressCounter].poseNext != Pose.Failed)
            {
                if (lastActiveCharacter != CharacterType.Failed)
                {
                    foreach (Pose nextPose in allPoses)
                    {
                        if (nextPose == dialogReader.dialogData.dialog[dialogProgressCounter].poseNext)
                        {
                            for (int i = 0; i < SpawnedCharacters.Count; i++)
                            {
                                if (SpawnedCharacters[i].myCharacterType == lastActiveCharacter)
                                {
                                    SpawnedCharacters[i].SetPose(nextPose);
                                }
                            }
                        }
                    }
                }
            }

            if (dialogReader.dialogData.dialog[dialogProgressCounter].alsoPose != CharacterType.Failed)
            {
                //Debug.Log("Also pose: "+ dialogReader.dialogData.dialog[dialogProgressCounter].alsoPose);
                for (int i = 0; i < SpawnedCharacters.Count; i++)
                {
                    if (SpawnedCharacters[i].myCharacterType == dialogReader.dialogData.dialog[dialogProgressCounter].alsoPose)
                    {
                        foreach (Pose nextPose in allPoses)
                        {
                            if (nextPose == dialogReader.dialogData.dialog[dialogProgressCounter].otherPose)
                            {
                                SpawnedCharacters[i].SetPose(nextPose);
                            }
                        }
                    }
                }
            }
        }
        dialogProgressCounter++;
        CharacterType speaker = 0;
        Pose speakingPose = 0;
        Staging speakerStage = 0;
        bool multiPartSpeech = false;
        string text1 = dialogReader.dialogData.dialog[dialogProgressCounter].text1;
        string text2 = dialogReader.dialogData.dialog[dialogProgressCounter].text2;
       
        foreach (CharacterType value in allCharacterTypes)
        {
            if (value == dialogReader.dialogData.dialog[dialogProgressCounter].speaker)
            {
                speaker = value;
            }
        }

        foreach (Pose aPose in allPoses)
        {
            if (aPose == dialogReader.dialogData.dialog[dialogProgressCounter].pose)
            {
                speakingPose = aPose;
            }
        }

        foreach (Staging aStage in allStages)
        {
            if (aStage == dialogReader.dialogData.dialog[dialogProgressCounter].stage)
            {
                speakerStage = aStage;
            }
        }

        if (!string.IsNullOrEmpty(text2))
        {
            multiPartSpeech = true;
        }
        
        DoCharacterDialog(speaker,speakingPose,speakerStage,multiPartSpeech,text1,text2);
        lastActiveCharacter = speaker;
        if (dialogReader.dialogData.dialog[dialogProgressCounter].imgFlash)
        {
            flasher.DoFlash();
        }
    }
    
    /// <summary>
    /// Make a character do some dialog. Creates the character if it doesn't already exist. Moves it to the correct stage
    /// </summary>
    /// <param name="whichCharacter"></param>
    /// <param name="whichPose"></param>
    /// <param name="where"></param>
    /// <param name="hasTwoParts"></param>
    /// <param name="dialogPart1"></param>
    /// <param name="dialogPart2"></param>
    public void DoCharacterDialog(CharacterType whichCharacter, Pose whichPose, Staging where, bool hasTwoParts, string dialogPart1, string dialogPart2 = null)
    {
        // Make sure the transform for the stage type exists
        if (characterPanelTransforms.TryGetValue(where, out Transform characterPanelTransform))
        {
            characterPanelTransform.gameObject.SetActive(true);
            Character characterInstance = SpawnCharacter(whichCharacter, characterPanelTransform, where);

            
            
            characterInstance.DoCharacterDialog(whichPose,where,hasTwoParts,dialogPart1,dialogPart2);
        }
    }

    /// <summary>
    /// Get the character instance
    /// </summary>
    /// <param name="whichCharacter"></param>
    /// <param name="parent"></param>
    /// <param name="where"></param>
    /// <returns>A Character or null if the character isn't found and can't spawn</returns>
    public Character SpawnCharacter(CharacterType whichCharacter, Transform parent, Staging where)
    {
        Character characterInstance = null;
            
        // Check to see if the character is already spawned
        bool isSpawned = false;
        for (int i = 0; i < SpawnedCharacters.Count; i++)
        {
            if (SpawnedCharacters[i].myCharacterType == whichCharacter)
            {
                isSpawned = true;
                characterInstance = SpawnedCharacters[i];
                MoveCharacter(whichCharacter,where);
            }
        }
            
        // Character isn't spawned so create him and add him to the list
        if (!isSpawned)
        {
            // Create an instance of the character prefab
            characterInstance = (Instantiate(characterPrefab, parent)).GetComponent<Character>();
            characterInstance.myCharacterType = whichCharacter;
            characterInstance.Initialize();
            SpawnedCharacters.Add(characterInstance);
            MoveCharacter(whichCharacter, where);
        }

        return characterInstance;
    }

    /// <summary>
    /// Move a character that's already spawned
    /// </summary>
    /// <param name="whichCharacter">Which Character to move</param>
    /// <param name="where">Where to move them</param>
    public void MoveCharacter(CharacterType whichCharacter, Staging where)
    {
        if(characterPanelTransforms.TryGetValue(where, out Transform characterPanelTransform))
        {
            for (int i = 0; i < SpawnedCharacters.Count; i++)
            {
                if (SpawnedCharacters[i].myCharacterType == whichCharacter)
                {
                    SpawnedCharacters[i].GetComponent<Transform>().SetParent(characterPanelTransform);
                    SpawnedCharacters[i].GetComponent<Transform>().localPosition = Vector3.zero;
                    // Make the character smaller if they're in the background
                    RectTransform characterTransform = SpawnedCharacters[i].GetComponent<RectTransform>();
                    if (where == Staging.BackgroundCenter || where == Staging.BackgroundLeft || where == Staging.BackgroundRight)
                    {
                        characterTransform.localScale = new Vector3(backgroundStagingScale,backgroundStagingScale,backgroundStagingScale);
                        SpawnedCharacters[i].DisableDialog();
                    }
                    else
                    {
                        characterTransform.localScale = Vector3.one;
                    }
                }
            }
        }
    }
}
