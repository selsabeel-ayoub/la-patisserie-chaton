using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavourBowls : MonoBehaviour
{
    OrderController orderController;
    ObjectController objectController;

    [SerializeField] int flavourType;
    int currentIndex = 0;


    private void Start()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
        objectController = GameObject.FindWithTag("objectController").GetComponent<ObjectController>();
    }

    private void OnMouseDown()
    {
        currentIndex = orderController.currentlyMade.Count;

        orderController.currentlyMade.Add(new List<int>());

        orderController.currentlyMade[currentIndex].Add(0); // screen
        orderController.currentlyMade[currentIndex].Add(flavourType);

        Debug.Log("cookie flavour chosen:" + orderController.currentlyMade[currentIndex][0]);

        objectController.BowlsToPan();
    }
}
