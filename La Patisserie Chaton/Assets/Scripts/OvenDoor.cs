using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : MonoBehaviour
{
    [HideInInspector] public bool isOpen = false;

    [SerializeField] ObjectController objectController;
    [SerializeField] OrderController orderController;

    [SerializeField] Animator ovenAnimator;

    [SerializeField] int secUntilGreen = 5;
    [SerializeField] int secUntilRed = 8;

    Coroutine lastRoutine;

    int cookieTypeIndex;
    int orderInOvenIndex;
    bool canTakeOut = false;

    private void Start()
    {
        cookieTypeIndex = orderController.cookieTypeIndex;
    }

    private void OnMouseDown()
    {
        if (isOpen) //Closing
        {
            ovenAnimator.SetBool("isOpening", false);
            isOpen = false;

            if (objectController.ovenEmpty == false)
            {
                lastRoutine = StartCoroutine(OvenCoroutine());
            }
        }
        else //Opening
        {
            if (objectController.ovenEmpty)
            {
                ovenAnimator.SetBool("isOpening", true);
                isOpen = true;
            }
            else if (canTakeOut)
            {
                canTakeOut = false;

                ovenAnimator.SetBool("isOpening", true);
                isOpen = true;

                StopCoroutine(lastRoutine);

                objectController.panToCreamRoom();
            }
        }
    }

    IEnumerator OvenCoroutine()
    {
        //instantiate oven timer prefab

        yield return new WaitForSeconds(secUntilGreen);
        Debug.Log("green");
        canTakeOut = true;

        yield return new WaitForSeconds(secUntilRed);
        Debug.Log("red");
        orderInOvenIndex = objectController.CheckScreenNum(3);
        orderController.currentlyMade[orderInOvenIndex][cookieTypeIndex] = -1;
        //must also change sprite
    }

}
