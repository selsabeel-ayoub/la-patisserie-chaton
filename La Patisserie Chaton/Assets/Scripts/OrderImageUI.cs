using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderImageUI : MonoBehaviour
{
    OrderController orderController;
    GameObject orderControlObj;

    [SerializeField] Sprite vanillaMacaron;
    [SerializeField] Sprite chocMacaron;
    [SerializeField] Sprite strawbMacaron;

    [SerializeField] Sprite vanillaCream;
    [SerializeField] Sprite chocCream;
    [SerializeField] Sprite strawbCream;

    int thisOrderNum = 0;
    Sprite cookieType;
    Sprite creamType;

    private void Awake()
    {
        orderControlObj = GameObject.FindWithTag("orderController");
        orderController = orderControlObj.GetComponent<OrderController>();

        thisOrderNum = orderController.takenOrders.Count - 1;

        //Cookie Type Image
        if (orderController.takenOrders[thisOrderNum][0] == 0)
        {
            cookieType = vanillaMacaron;
        }
        else if (orderController.takenOrders[thisOrderNum][0] == 1)
        {
            cookieType = chocMacaron;
        }
        else
        {
            cookieType = strawbMacaron;
        }

        //Cream Type Image
        if (orderController.takenOrders[thisOrderNum][1] == 0)
        {
            creamType = vanillaCream;
        }
        else if (orderController.takenOrders[thisOrderNum][1] == 1)
        {
            creamType = chocCream;
        }
        else
        {
            creamType = strawbCream;
        }

        //Setting the Images
        gameObject.transform.GetChild(1).GetComponent<Image>().sprite = cookieType;
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = creamType;
    }
}
