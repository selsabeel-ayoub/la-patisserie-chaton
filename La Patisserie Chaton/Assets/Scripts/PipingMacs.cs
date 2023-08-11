using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipingMacs : MonoBehaviour
{
    [SerializeField] ObjectController objectController;
    bool pipeOnPan = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ovenPan")
        {
            pipeOnPan = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ovenPan")
        {
            pipeOnPan = false;
        }
    }


    private void OnMouseUp()
    {
        if (pipeOnPan == true)
        {
            objectController.PipeMacaron();
        }
    }
}
