using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class OrderSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public static int selectedOrderNum = -1;

    public void OnSelect(BaseEventData eventData)
    {
        selectedOrderNum = transform.GetSiblingIndex();
        Debug.Log(selectedOrderNum + " was selected");
    }

    public void OnDeselect (BaseEventData eventData)
    {
        selectedOrderNum = -1;
        Debug.Log("deselected");
    }
}
