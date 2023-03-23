using UnityEngine;
using UnityEngine.EventSystems;

public class TouchOrClickHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public static event System.Action OnClickEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickEvent != null)
        {
            OnClickEvent();
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
}

