using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderOnClick : MonoBehaviour
{
    public OrderController orderController;
    private void Awake()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
    }
    private void OnMouseDown()
    {
        orderController.TakeOrder();

        //play animation
    }
}
