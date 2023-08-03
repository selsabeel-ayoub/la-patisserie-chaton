using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : MonoBehaviour
{
    [SerializeField] Animator ovenAnimator;

    bool isOpen = false;
    //maybe smth to know if an anim is currently happening?

    private void OnMouseDown()
    {
        if (isOpen)
        {
            ovenAnimator.SetBool("isOpening", false);
            isOpen = false;

            //if pan is inside, start coroutine (timer and things)
        }
        else
        {
            ovenAnimator.SetBool("isOpening", true);
            isOpen = true;
        }
    }

    //if burn it, change currently made cookie flavour to -1
}
