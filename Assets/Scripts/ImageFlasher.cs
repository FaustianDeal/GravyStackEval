using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFlasher : MonoBehaviour
{
    public Image myImage;
    public float flashInterval = 0.5f;
    //public int flashCount = 3;
    public float alphaMax = 0.8f;

    private Coroutine flashCoroutine;

    public event Action ImageFlashComplete;

    public void DoFlash()
    {
        flashCoroutine = StartCoroutine(FlashImage());
    }

    private void OnDestroy()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
    }
    
    private IEnumerator FlashImage()
    {
        Color originalColor = myImage.color;
        //for (int i = 0; i < flashCount; i++)
        //{
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / flashInterval;
                float alpha = Mathf.Lerp(originalColor.a, alphaMax, t);
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                myImage.color = newColor;
                yield return null;
            }

            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / flashInterval;
                float alpha = Mathf.Lerp(alphaMax, originalColor.a, t);
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                myImage.color = newColor;
                yield return null;
            }
        //}

        myImage.color = originalColor;
        ImageFlashComplete?.Invoke();
    }
}