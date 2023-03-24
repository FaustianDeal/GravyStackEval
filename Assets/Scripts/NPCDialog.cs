using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private List<ChatBubble> ChatBubbles;

    public void DoDialog(bool hasTwoParts, string textToDisplayPart1, Staging staging, string textToDisplayPart2 = null)
    {
        for (int i = 0; i < ChatBubbles.Count; i++)
        {
            ChatBubbles[i].gameObject.SetActive(false);
        }
        if (!hasTwoParts)   // Use simple chat bubble
        {
            ChatBubbles[0].gameObject.SetActive(true);
            ChatBubbles[0].AddDialogText(false,textToDisplayPart1);
            if (staging == Staging.BackgroundRight
                || staging == Staging.ForegroundRight
                || staging == Staging.BackgroundCenter
                || staging == Staging.ForegroundCenter)
            {
                // Flip the bubble for staging on the right or Center
                Flip(ChatBubbles[0].gameObject,false);
            }
            else
            {
                Flip(ChatBubbles[0].gameObject,true);
            }
        }
        else  // Use dual chat bubble
        {
            ChatBubbles[1].gameObject.SetActive(true);
            ChatBubbles[1].AddDialogText(true,textToDisplayPart1,textToDisplayPart2);
            if (staging == Staging.BackgroundLeft
                || staging == Staging.ForegroundLeft)
            {
                // Flip the bubble for staging on the right or Center
                Flip(ChatBubbles[0].gameObject,false);
            }
            else
            {
                Flip(ChatBubbles[0].gameObject,true);
            }
        }
    }
    
    void Flip(GameObject bubble, bool normal)
    {
        int flipValue = -1;
        if (normal)
        {
            flipValue = 1;
        }
        Vector3 newScale = bubble.transform.localScale;
        newScale.x = flipValue;
        bubble.transform.localScale = newScale;

        foreach (Transform child in bubble.transform)
        {
            Vector3 childScale = child.localScale;
            childScale.x = flipValue;
            child.localScale = childScale;
        }
    }
}
