using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    public List<Image> ChatBubbles;
    public List<TextMeshProUGUI> TextAreas;
    [TextArea] public string TextToDisplay;
}
