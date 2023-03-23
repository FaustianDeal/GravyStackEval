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
            if (staging == Staging.BackgroundRight
                || staging == Staging.ForegroundRight
                || staging == Staging.BackgroundCenter
                || staging == Staging.ForegroundCenter)
            {
                // Flip the bubble for staging on the right or Center
                Flip(ChatBubbles[0].gameObject);
            }
            ChatBubbles[0].AddDialogText(false,textToDisplayPart1);
        }
        else  // Use dual chat bubble
        {
            ChatBubbles[1].gameObject.SetActive(true);
            if (staging == Staging.BackgroundLeft
                || staging == Staging.ForegroundLeft)
            {
                // Flip the bubble for staging on the right or Center
                Flip(ChatBubbles[0].gameObject);
            }
            ChatBubbles[1].AddDialogText(true,textToDisplayPart1,textToDisplayPart2);
        }
    }
    
    void Flip(GameObject bubble)
    {
        Vector3 newScale = bubble.transform.localScale;
        newScale.y *= -1;
        bubble.transform.localScale = newScale;
    }
}
