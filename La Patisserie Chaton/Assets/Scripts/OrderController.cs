using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderController : MonoBehaviour
{
    [SerializeField] Button orderButton;
    [SerializeField] Transform canvasPanelTransform;

    [SerializeField] Vector2 orderUIPosition = new Vector2(50, 330);

    //2D Lists
    List<List<int>> newOrders = new List<List<int>>();
    List<List<int>> takenOrders = new List<List<int>>();
    List<List<int>> currentlyMade = new List<List<int>>();

    List<int> selectedOrder = new List<int>();

    int newOrderIndex = 0;


    //These are orders that have not been taken yet
    [ContextMenu("Add New Order")]
    void NewOrder() // call this funcion in update of fixed update after a certain amount of time
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

    [ContextMenu("Take Order")]
    void takeOrder ()
    {
        //when customer is pressed
        //add to order time it was taken

        Instantiate(orderButton, orderUIPosition, Quaternion.identity, canvasPanelTransform); //(object, pos, roation, parent)
        orderUIPosition += new Vector2(60, 0);


        int item1 = newOrders[0][0]; //cookie type
        int item2 = newOrders[0][1]; //cream type

        int listLen = takenOrders.Count;

        takenOrders.Add(new List<int>());

        takenOrders[listLen].Add(item1);
        takenOrders[listLen].Add(item2);

        newOrders.RemoveAt(0);
        newOrderIndex -= 1;

        Debug.Log("cookie type: " + takenOrders[listLen][0] + " cream type:" + takenOrders[listLen][1]);
    }

    void SellOrder ()
    {
        //only here set selectedOrder = takenOrders[selectedOrderNum] bc its only needed here so fo performance
        //selectedOrder = takenOrders[selectedOrderNum]; - get selectedOrderNum from other script

        //WHEN DELETING ORDER HAVE TO -1 FROM X POS OF orderUIPosition --> orderUIPosition -= new Vector2(60, 0);
        //have to move all the ui after it too sighghghgh
    }
}
