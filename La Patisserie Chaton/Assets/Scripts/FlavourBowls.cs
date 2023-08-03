using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavourBowls : MonoBehaviour
{
    OrderController orderController;

    [SerializeField] int flavourType;
    int currentIndex = 0;


    private void Start()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
    }

    private void OnMouseDown()
    {
        currentIndex = orderController.currentlyMade.Count;

        orderController.currentlyMade.Add(new List<int>());
        orderController.currentlyMade[currentIndex].Add(flavourType);

        Debug.Log("cookie flavour chosen:" + orderController.currentlyMade[currentIndex][0]);

        //disable the triggercolliders of all of them once one is chosen, remmeber to re-enable when this one is over on next screen
    }
}
