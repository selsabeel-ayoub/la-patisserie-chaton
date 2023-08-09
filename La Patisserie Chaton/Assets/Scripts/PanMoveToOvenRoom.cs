using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanMoveToOvenRoom : MonoBehaviour
{
    [SerializeField] ObjectController objectController;

    private void OnMouseDown()
    {
        if (objectController.areMacsPiped)
        {
            objectController.PanToOvenScreen();
        }
    }
}
