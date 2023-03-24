using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 1f;

    private void Awake()
    {
        // Start fading the image out immediately when the script is attached to the object
        FadeOut();
    }

    public void FadeOut()
    {
        imageToFade.CrossFadeAlpha(0f, .01f, true);
    }

    public void FadeIn()
    {
        // FadeOut();
        // this.RunDelayed(1f, () =>
        // {
        //     imageToFade.CrossFadeAlpha(1f, fadeDuration, true);
        // });
        imageToFade.CrossFadeAlpha(1f, fadeDuration, true);
    }
}

public static class MonoBehaviourExt
{
    public static IEnumerator DelayedCoroutine(this MonoBehaviour mb, float delay, System.Action a)
    {
        yield return new WaitForSeconds(delay);
        a();
    }

    /// <summary>
    /// this.RunDelayed(2f,() =>
    /// {
    ///     Debug.Log("Whatever");
    /// });
    /// </summary>
    /// <param name="mb"></param>
    /// <param name="delay"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Coroutine RunDelayed(this MonoBehaviour mb, float delay, System.Action a)
    {
        return mb.StartCoroutine(mb.DelayedCoroutine(delay, a));
    }
}