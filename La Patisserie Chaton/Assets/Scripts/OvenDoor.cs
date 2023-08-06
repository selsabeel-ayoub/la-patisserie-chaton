using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : MonoBehaviour
{
    public bool isOpen = false;

    [SerializeField] Animator ovenAnimator;

    //maybe smth to know if an anim is currently happening?

    private void OnMouseDown()
    {
        if (isOpen)
        {
            ovenAnimator.SetBool("isOpening", false);
            isOpen = false;

            //if ovenEmpty = false, start coroutine (timer and things)
            //MovetoOven () fnc from object controller
        }
        else
        {
            ovenAnimator.SetBool("isOpening", true);
            isOpen = true;

            //if u open it when coroutine has started, it stops the coroutine, takes out the pan, sets ovenempty = true
            //must change screen index
        }
    }


    //if burn it, change currently made cookie flavour to -1
}
