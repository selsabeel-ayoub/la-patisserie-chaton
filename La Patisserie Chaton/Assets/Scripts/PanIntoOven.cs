using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanIntoOven : MonoBehaviour
{
    ObjectController objectController;
    DragAndDrop dragAndDrop;
    OvenDoor ovenDoor;

    bool isPanInOven;

    private void Awake()
    {
        objectController = GameObject.FindWithTag("objectController").GetComponent<ObjectController>();
        ovenDoor = GameObject.FindWithTag("ovenDoor").GetComponent<OvenDoor>();
        dragAndDrop = GetComponent<DragAndDrop>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ovenDoor")
        {
            isPanInOven = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ovenDoor")
        {
            isPanInOven = false;
        }
    }

    private void OnMouseUp()
    {
        if (isPanInOven && objectController.ovenEmpty && ovenDoor.isOpen)
        {
            dragAndDrop.enabled = false;
            transform.position = new Vector2(41, -2);

            objectController.MoveInOven();
        }
    }
}
