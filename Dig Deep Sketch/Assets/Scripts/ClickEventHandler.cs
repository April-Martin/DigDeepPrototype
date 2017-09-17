using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public UnityEvent leftClickDown;
    public UnityEvent rightClickDown;
    public UnityEvent leftClickUp;
    public UnityEvent rightClickUp;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClickDown.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClickDown.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClickUp.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClickUp.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {

        }
    }
}
