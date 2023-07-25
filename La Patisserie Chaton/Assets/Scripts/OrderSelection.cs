using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class OrderSelection : MonoBehaviour, ISelectHandler
{
    int selectedOrderNum;

    public void OnSelect(BaseEventData eventData)
    {
        selectedOrderNum = transform.GetSiblingIndex();
        Debug.Log(selectedOrderNum + " was selected");
    }
}
