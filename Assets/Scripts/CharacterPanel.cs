using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    public List<Character> characterPrefabs; // The character prefab that you want to place
    public List<Transform> characterForegroundPositions; // List of foreground positions characters can appear in
    public List<Transform> characterBackgroundPositions;// List of background positions characters can appear in
    public float backgroundStagingScale = .75f;
    private Vector2 positionOnCanvas; // The position on the canvas where you want to place the image

    private Character characterInstance; // The instance of the character that was be placed
    public enum Staging // Where a character will appear in the scene
    {
        ForegroundLeft,
        ForegroundCenter,
        ForegroundRight,
        BackgroundLeft,
        BackgroundCenter,
        BackgroundRight
    }
    void Start()
    {
        // Create an instance of the character prefab
        //characterInstance = Instantiate(characterPrefab, characterPanelTransform);

        // Set the position of the character on the canvas
        //characterInstance.anchoredPosition = positionOnCanvas;
    }
}
