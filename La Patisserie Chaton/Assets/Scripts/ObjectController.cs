using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectController : MonoBehaviour
{
    [SerializeField] OrderController orderController;
    [SerializeField] BoxCollider2D[] flavBowlColliders;
    [SerializeField] PlayableDirector bowlsToPanPlayable;

    //change later to just one gameobject with sprite changing
    [SerializeField] GameObject vanPan;
    [SerializeField] GameObject chocPan;
    [SerializeField] GameObject strawPan;


    int mFlavorNum = 3;
    //int cFlavorNum = 3;

    int screenIndex;
    int firstInOvenBacklog; // better name?
    int screenNum;

    bool ovenEmpty = true;
    bool ovenBacklog = false;

    private void Start()
    {
        screenIndex = orderController.screenIndex;
    }

    public void BowlsToPan ()
    {
        for (int x = 0; x < mFlavorNum; x++)
        {
            flavBowlColliders[x].enabled = false;
        }

        bowlsToPanPlayable.Play();
    }

    //pan to oven screen, must change the screenIndex of last item in list to 1 (or check which ones is screen 0 and change that)
    //call NextOvenSlot()


    //THIS MUST BE TESTED STILl!!!! - is x the correct number - is the next one up for ove nslot the correct order - do all screenindexes change properly
    void MoveInOven ()
    {
        //gets triggered when trigger collider of pan is over oven + oven is open
        if (ovenEmpty == true)
        {
            NextOvenSlot();
            //only if acc goes thru
            orderController.currentlyMade[firstInOvenBacklog - 1][screenIndex] = 3; //change the object going into oven's screen's # (2 -> 3)
        }
    }


    void NextOvenSlot ()
    {
        int madeListLen = orderController.currentlyMade.Count;

        for (int x = 0; x < madeListLen; x++)
        {
            if (orderController.currentlyMade[x][screenIndex] == 1) //check if current index index is in oven backlog
            {
                ovenBacklog = true;

                firstInOvenBacklog = x;
                break;
            }
        }


        if (ovenBacklog == true) //if there is a backlog, move the first in backlog to the oven slot
        {
            ovenBacklog = false;

            Debug.Log("(orderNum that is next up for oven slot): " + firstInOvenBacklog);

            //Check cookie flavour of the order next up for the oven slot
            if (orderController.currentlyMade[firstInOvenBacklog][1] == 0)
            {
                Instantiate(vanPan);
            }
            else if (orderController.currentlyMade[firstInOvenBacklog][1] == 1)
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



    int CheckScreenNum (screenNum)
    {
        int madeListLen = orderController.currentlyMade.Count;

        for (int x = 0; x < madeListLen; x++)
        {
            if (orderController.currentlyMade[x][screenIndex] == screenNum) //check if current index index is in a certain screen
            {
                return x;
            }
        }
    }
}
