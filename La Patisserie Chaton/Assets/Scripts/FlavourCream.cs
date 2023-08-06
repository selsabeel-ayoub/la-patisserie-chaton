using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavourCream : MonoBehaviour
{
    OrderController orderController;

    [SerializeField] int flavourType;

    private void Start()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
    }

    private void OnMouseDown()
    {
        //only let choose cream if there is a cookie on the screen
        orderController.currentlyMade[0].Add(flavourType);
        Debug.Log("cream flavour chosen:" + orderController.currentlyMade[0][2]);
    }
}
