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
        BronzeBear,
        GreenGecko,
        PurplePenguin
    }
    public enum Pose    // What pose the character should use
    {
        Pose1,
        Pose2,
        Pose3
    }
    public enum Staging // Where a character will appear in the scene
    {
        ForegroundLeft,
        ForegroundCenter,
        ForegroundRight,
        BackgroundLeft,
        BackgroundCenter,
        BackgroundRight
    }
public class CharacterPanel : MonoBehaviour
{
    public GameObject characterPrefab; // The character prefab that you want to place
    
    public float backgroundStagingScale = .75f;
    private Dictionary<Staging, Transform> characterPanelTransforms;
    private List<Character> SpawnedCharacters;
    
    void Start()
    {
        characterPanelTransforms = new Dictionary<Staging, Transform>();
        SpawnedCharacters = new List<Character>();
        List<Transform> characterForegroundPositions = new List<Transform>();
        List<Transform> characterBackgroundPositions = new List<Transform>();
        
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
        Staging[] stagings = (Staging[])Enum.GetValues(typeof(Staging));
        for (int i = 0; i < stagings.Length; i++)
        {
            Transform tfm = null;
    
            if (i < characterForegroundPositions.Count && stagings[i] >= Staging.ForegroundLeft && stagings[i] <= Staging.ForegroundRight)
            {
                tfm = characterForegroundPositions[i];
            }
            else if (i < characterForegroundPositions.Count + characterBackgroundPositions.Count && stagings[i] >= Staging.BackgroundLeft && stagings[i] <= Staging.BackgroundRight)
            {
                tfm = characterBackgroundPositions[i - characterForegroundPositions.Count];
            }

            characterPanelTransforms.Add(stagings[i], tfm);
        }
        DoCharacterDialog(CharacterType.GreenGecko, Pose.Pose1, Staging.ForegroundCenter, true, "Testing Part 1", "Testing Part 2");
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
