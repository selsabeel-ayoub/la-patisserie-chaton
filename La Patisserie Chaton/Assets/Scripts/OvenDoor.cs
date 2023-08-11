using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : MonoBehaviour
{
    [HideInInspector] public bool isOpen = false;

    [SerializeField] ObjectController objectController;
    [SerializeField] OrderController orderController;

    [SerializeField] GameObject ovenTimer;
    [SerializeField] Animator ovenAnimator;
    [SerializeField] Transform inOvenObj;
    [SerializeField] Sprite burntOvenPan;

    int secUntilGreen = 6;
    int secUntilRed = 3;

    Coroutine lastRoutine;
    GameObject ovenTimerObj;
    SpriteRenderer ovenPanSprite;

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
                Destroy(ovenTimerObj);

                objectController.panToCreamRoom();
            }
        }
    }

    IEnumerator OvenCoroutine() //Oven Timer
    {
        yield return new WaitForSeconds(1);
        ovenTimerObj = Instantiate(ovenTimer);

        yield return new WaitForSeconds(secUntilGreen);
        canTakeOut = true;

        yield return new WaitForSeconds(secUntilRed);
        orderInOvenIndex = objectController.CheckScreenNum(3);
        orderController.currentlyMade[orderInOvenIndex][cookieTypeIndex] = -1;
        ovenPanSprite = inOvenObj.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        ovenPanSprite.sprite = burntOvenPan;
    }

}
