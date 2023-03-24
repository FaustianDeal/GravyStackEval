using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ChatBubble : MonoBehaviour
{
    public List<TextMeshProUGUI> TextAreas;

    private RectTransform rectTransform;
    private Rect safeArea;
    private RectTransform safeAreaRectTransform;

    private bool initialized = false;
    void Initialize()
    {
        rectTransform = GetComponent<Image>().GetComponent<RectTransform>();
        safeArea = Screen.safeArea;
        safeAreaRectTransform = new GameObject("SafeArea", typeof(RectTransform)).GetComponent<RectTransform>();
        safeAreaRectTransform.SetParent(rectTransform.parent);
        safeAreaRectTransform.position = new Vector2(Screen.safeArea.x + Screen.safeArea.width / 2, Screen.safeArea.y + Screen.safeArea.height / 2);
        safeAreaRectTransform.sizeDelta = new Vector2(Screen.safeArea.width, Screen.safeArea.height);
    }
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

        if (!initialized)
        {
            Initialize();
            initialized = true;
        }
        CheckOverlap(rectTransform,safeAreaRectTransform);
    }

    void CheckOverlap(RectTransform imageTransform, RectTransform parentRectTransform)
    {
        // Calculate the corners of the image and parent RectTransform in world space
        Vector3[] imageCorners = new Vector3[4];
        imageTransform.GetWorldCorners(imageCorners);

        Vector3[] parentCorners = new Vector3[4];
        parentRectTransform.GetWorldCorners(parentCorners);

        // Check if any of the image corners are outside the parent RectTransform
        bool isOutside = false;
        foreach (Vector3 imageCorner in imageCorners)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(parentRectTransform, imageCorner))
            {
                Debug.Log("CHAT OUTSIDE");
                isOutside = true;
                break;
            }
        }

        // If the image is outside the parent RectTransform, move it back inside
        if (isOutside)
        {
            Vector3 newPosition = imageTransform.position;

            if (imageCorners[0].x < parentCorners[0].x) // Left edge outside
            {
                newPosition.x += parentCorners[0].x - imageCorners[0].x;
            }
            else if (imageCorners[2].x > parentCorners[2].x) // Right edge outside
            {
                newPosition.x -= imageCorners[2].x - parentCorners[2].x;
            }

            if (imageCorners[0].y < parentCorners[0].y) // Bottom edge outside
            {
                newPosition.y += parentCorners[0].y - imageCorners[0].y;
            }
            else if (imageCorners[1].y > parentCorners[1].y) // Top edge outside
            {
                newPosition.y -= imageCorners[1].y - parentCorners[1].y;
            }

            imageTransform.position = newPosition;
        }
    }
    
}
