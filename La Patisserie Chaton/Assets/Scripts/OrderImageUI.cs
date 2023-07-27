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
        if (orderController.takenOrders[thisOrderNum][0] == 1)
        {
            Debug.Log("cookies vanille");
            cookieType = vanillaMacaron;
        }
        else if (orderController.takenOrders[thisOrderNum][0] == 2)
        {
            cookieType = chocMacaron;
            Debug.Log("cookies choc");
        }
        else
        {
            cookieType = strawbMacaron;
            Debug.Log("cookies straw");
        }

        //Cream Type Image
        if (orderController.takenOrders[thisOrderNum][1] == 1)
        {
            creamType = vanillaCream;
            Debug.Log("creams vanille");
        }
        else if (orderController.takenOrders[thisOrderNum][1] == 2)
        {
            creamType = chocCream;
            Debug.Log("creams choc");
        }
        else
        {
            creamType = strawbCream;
            Debug.Log("creams straw");
        }

        //Setting the Images
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = cookieType;
        gameObject.transform.GetChild(1).GetComponent<Image>().sprite = creamType;
    }
}
