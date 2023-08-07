using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipingCream : MonoBehaviour
{
    [SerializeField] OrderController orderController;
    [SerializeField] ObjectController objectController;

    [SerializeField] int flavourType;

    bool cremOnPan = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "cremPan")
        {
            cremOnPan = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "cremPan")
        {
            cremOnPan = false;
        }
    }


    private void OnMouseUp()
    {
        if (cremOnPan == true)
        {
            orderController.currentlyMade[0].Add(flavourType);
            Debug.Log("cream flavour chosen:" + orderController.currentlyMade[0][2]);

            objectController.AddCream();
        }
    }

}
