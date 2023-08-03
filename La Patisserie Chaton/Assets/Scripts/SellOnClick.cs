using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellOnClick : MonoBehaviour
{
    OrderController orderController;

    private void Start()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
    }
    private void OnMouseDown()
    {
        orderController.SellOrder();
    }
}
