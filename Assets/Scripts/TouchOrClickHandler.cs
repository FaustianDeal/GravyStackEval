using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchOrClickHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public static event System.Action OnClickEvent;

    private bool canClick = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick && OnClickEvent != null)
        {
            OnClickEvent();
            canClick = false;
            StartCoroutine(WaitForClick());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Touched/Clicked Down!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Touched/Clicked Up!");
    }

    private IEnumerator WaitForClick()
    {
        yield return new WaitForSeconds(1f);
        canClick = true;
    }
}

