using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    public List<TextMeshProUGUI> TextAreas;

    public void AddDialogText(bool twoParts, string textToAddPart1, string textToAddPart2 = null)
    {
        if (!twoParts)
        {
            TextAreas[0].text = textToAddPart1;
        }
        else
        {
            TextAreas[0].text = textToAddPart1;
            TextAreas[1].text = textToAddPart2;
        }
    }
}
