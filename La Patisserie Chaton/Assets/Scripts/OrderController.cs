using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderController : MonoBehaviour, ISelectHandler
{
    //2D Lists
    List<List<int>> newOrders = new List<List<int>>();
    List<List<int>> currentOrders = new List<List<int>>();
    List<List<int>> currentlyMade = new List<List<int>>();

    List<int> selectedOrder = new List<int>();

    int selectedOrderNum;


    int newOrderIndex = 0;


    public void OnSelect(BaseEventData eventData)
    {
        selectedOrderNum = transform.GetSiblingIndex();
        Debug.Log(selectedOrderNum + " was selected");
    }


    //These are orders that have not been taken yet
    [ContextMenu("Add New Order")]
    public void NewOrder() // call this funcion in update of fixed update after a certain amount of time
    {
        //add DING UI appearing here

        int cookieType = Random.Range(1, 4); // 1 = vanilla, 2 = choc, 3 = strawb
        int creamType = Random.Range(1, 4); // 1 = vanilla, 2 = choc, 3 = strawb

        newOrders.Add(new List<int>());
        newOrders[newOrderIndex].Add(cookieType);
        newOrders[newOrderIndex].Add(creamType);

        Debug.Log(newOrderIndex + "cookie type: " + newOrders[newOrderIndex][0] + "cream type:" + newOrders[newOrderIndex][1]);
        newOrderIndex++;
    }

    void takeOrder ()
    {
        //once buton is pressed to take order, make ui appear, create new list in currentOrders, copy newOrder[0] to currentOrders, then delete newOrder[0]
        //must also -1 from newOrderIndex!!
    }

    void SellOrder ()
    {
        //only here set selectedOrder = currentOrders[selectedOrderNum] bc its only needed here so fo performance
        selectedOrder = currentOrders[selectedOrderNum];
    }




}
