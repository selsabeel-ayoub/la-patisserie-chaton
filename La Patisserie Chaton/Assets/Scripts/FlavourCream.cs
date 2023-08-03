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
        orderController.currentlyMade[0].Add(flavourType);
        Debug.Log("cream flavour chosen:" + orderController.currentlyMade[0][1]);

        //disable the triggercolliders of all of them once one is chosen, remmeber to re-enable when sell
    }
}
