using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class OrderSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public static int selectedOrderNum = -1;

    public void OnSelect(BaseEventData eventData)
    {
        selectedOrderNum = transform.parent.transform.GetSiblingIndex();
    }

    public void OnDeselect (BaseEventData eventData)
    {
        selectedOrderNum = -1;
    }
}
