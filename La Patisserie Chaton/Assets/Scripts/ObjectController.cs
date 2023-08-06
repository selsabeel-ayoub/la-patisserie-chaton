using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectController : MonoBehaviour
{
    public bool ovenEmpty = true;

    [SerializeField] OrderController orderController;
    [SerializeField] BoxCollider2D[] flavBowlColliders;
    [SerializeField] PlayableDirector bowlsToPanPlayable;

    //change later to just one gameobject with sprite changing (Gameobject ovenPan)
    [SerializeField] GameObject vanPan;
    [SerializeField] GameObject chocPan;
    [SerializeField] GameObject strawPan;

    int screenIndex;
    int macTypeIndex;
    //int cremTypeIndex;

    int macFlavorNum = 3;
    //int cremFlavorNum = 3;

    int firstInOvenBacklog; // better name?


    private void Start()
    {
        screenIndex = orderController.screenIndex;
        macTypeIndex = orderController.cookieTypeIndex_made;
        //cremTypeIndex = orderController.creamTypeIndex_made;
    }

    public void BowlsToPan ()
    {
        for (int x = 0; x < macFlavorNum; x++)
        {
            flavBowlColliders[x].enabled = false;
        }

        bowlsToPanPlayable.Play();
    }

    [ContextMenu ("Pan to Oven Screen")]
    void PanToOvenScreen ()
    {
        orderController.currentlyMade[CheckScreenNum(0)][screenIndex] = 1; //change screen num (0 -> 1)
        NextOvenSlot();
    }


    void NextOvenSlot ()
    {
        if (CheckScreenNum(2) == -1) //only if there isnt anything in the oven slot right now
        {
            firstInOvenBacklog = CheckScreenNum(1); //check which index is in oven backlog (screen 1). and set to firstInOvenBacklog


            if (firstInOvenBacklog != -1) //if there is a backlog, move the first in backlog to the oven slot
            {
                //Check cookie flavour of the order next up for the oven slot
                if (orderController.currentlyMade[firstInOvenBacklog][macTypeIndex] == 0)
                {
                    Instantiate(vanPan);
                }
                else if (orderController.currentlyMade[firstInOvenBacklog][macTypeIndex] == 1)
                {
                    Instantiate(chocPan);
                }
                else
                {
                    Instantiate(strawPan);
                }

                orderController.currentlyMade[firstInOvenBacklog][screenIndex] = 2; //change the object going into oven slot's screen # (1 -> 2)
            }
        }
    }

    public void MoveInOven()
    {
        ovenEmpty = false;
        orderController.currentlyMade[CheckScreenNum(2)][screenIndex] = 3; //change the screen # of object going into oven (2 -> 3)

        NextOvenSlot();
    }



    int CheckScreenNum (int screenNum) //returns the index of the first order on a specific screen
    {
        int madeListLen = orderController.currentlyMade.Count;

        for (int x = 0; x < madeListLen; x++)
        {
            if (orderController.currentlyMade[x][screenIndex] == screenNum)
            {
                return x;
            }
        }

        return -1; // if there is no order on this screen
    }
}
