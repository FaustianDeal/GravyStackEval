using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LetterboxEffect : MonoBehaviour
{
    [SerializeField] private Image topLetterbox;
    [SerializeField] private Image bottomLetterbox;
    [SerializeField] private float fadeDuration = 1f;
    
    private Coroutine currentCoroutine;

    // private void Start()
    // {
    //     // Set the letterbox images to the same width as the UI image
    //     topLetterbox.rectTransform.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, 0);
    //     bottomLetterbox.rectTransform.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, 0);
    // }

    public void ShowLetterbox()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeLetterbox(1f));
    }

    public void HideLetterbox()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeLetterbox(0f));
    }

    private IEnumerator FadeLetterbox(float targetAlpha)
    {
        float startTime = Time.time;
        float startAlpha = topLetterbox.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            topLetterbox.color = new Color(0f, 0f, 0f, alpha);
            bottomLetterbox.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        topLetterbox.color = new Color(0f, 0f, 0f, targetAlpha);
        bottomLetterbox.color = new Color(0f, 0f, 0f, targetAlpha);
    }
}