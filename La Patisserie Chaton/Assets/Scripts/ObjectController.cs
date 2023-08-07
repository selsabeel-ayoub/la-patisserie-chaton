using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectController : MonoBehaviour
{
    public bool ovenEmpty = true;

    [SerializeField] OrderController orderController;

    [SerializeField] BoxCollider2D[] flavBowlColliders;
    [SerializeField] BoxCollider2D[] cremColliders;
    [SerializeField] BoxCollider2D panCollider;
    [SerializeField] Transform inOvenObj;

    [SerializeField] PlayableDirector bowlsToPanPlayable;
    [SerializeField] PlayableDirector panToOvenRoomPlayable;
    [SerializeField] PlayableDirector panToCreamRoomPlayable;

    //change later to just one gameobject with sprite changing (Gameobject ovenPan)
    [SerializeField] GameObject vanPan_oven;
    [SerializeField] GameObject chocPan_oven;
    [SerializeField] GameObject strawPan_oven;
    [SerializeField] GameObject vanPan_crem;
    [SerializeField] GameObject chocPan_crem;
    [SerializeField] GameObject strawPan_crem;
    [SerializeField] GameObject burntPan_crem;

    int ovenBacklogScreen = 1;
    int ovenSlotScreen = 2;
    int cremBacklogScreen = 4;
    int cremSlotScreen = 5;

    /*Screens: 0 - mixing room
               1 - waiting for oven slot
               2 - in oven slot
               3 - in oven
               4 - waiting for cream slot
               5 - in cream slot */

    int screenIndex;
    int macTypeIndex;

    int macFlavorNum = 3;
    int cremFlavorNum = 3;

    int firstInOvenBacklog;
    Vector3 inOvenObjStartPos;

    float panToCreamRoomPlayableLen = 2f;


    private void Start()
    {
        screenIndex = orderController.screenIndex;
        macTypeIndex = orderController.cookieTypeIndex_made;

        inOvenObjStartPos = inOvenObj.position;
    }

    public void BowlsToPan ()
    {
        for (int x = 0; x < macFlavorNum; x++)
        {
            flavBowlColliders[x].enabled = false;
        }

        panCollider.enabled = true;
        bowlsToPanPlayable.Play();
    }

    public void PanToOvenScreen ()
    {
        panCollider.enabled = false;
        panToOvenRoomPlayable.Play();

        for (int x = 0; x < macFlavorNum; x++)
        {
            flavBowlColliders[x].enabled = true;
        }

        orderController.currentlyMade[CheckScreenNum(0)][screenIndex] = 1; //change screen num (0 -> 1)
        NextInOvenSlot();
    }


    public void MoveInOven()
    {
        ovenEmpty = false;
        orderController.currentlyMade[CheckScreenNum(2)][screenIndex] = 3; //change the screen # of object going into oven (2 -> 3)

        NextInOvenSlot();
    }

    public void panToCreamRoom ()
    {
        ovenEmpty = true;
        orderController.currentlyMade[CheckScreenNum(3)][screenIndex] = 4; //change the screen # of object going into cream room backlog (3 -> 4)

        panToCreamRoomPlayable.Play();
        StartCoroutine(ResetInOvenObj());
        NextCreamSlot();
    }

    IEnumerator ResetInOvenObj ()
    {
        yield return new WaitForSeconds(panToCreamRoomPlayableLen);
        Destroy(inOvenObj.GetChild(0).gameObject);
    }

    public void AddCream()
    {
        for (int x = 0; x < cremFlavorNum; x++)
        {
           cremColliders[x].enabled = false;
        }

        //add cream sprites using 
        //play closing anim
        //set bool to true to allow to sell now that anim is done
    }

    public void ResetCream ()
    {
        for (int x = 0; x < cremFlavorNum; x++)
        {
            cremColliders[x].enabled = true;
        }
    }

    void NextInOvenSlot()
    {
        NextInSlot(ovenSlotScreen, ovenBacklogScreen, vanPan_oven, chocPan_oven, strawPan_oven, null);
    }

    public void NextCreamSlot()
    {
        NextInSlot(cremSlotScreen, cremBacklogScreen, vanPan_crem, chocPan_crem, strawPan_crem, burntPan_crem);
    }


    void NextInSlot(int slotScreenNum, int backlogScreenNum, GameObject vanPan, GameObject chocPan, GameObject strawPan, GameObject burntPan)
    {
        if (CheckScreenNum(slotScreenNum) == -1) //only if there isnt anything in the oven slot right now
        {
            int firstInBacklog = CheckScreenNum(backlogScreenNum); //check which index of list is first in backlog and set to firstInBacklog

            if (firstInBacklog != -1) //if there is a backlog, move the first in backlog to the slot
            {
                //Check cookie flavour of the order next up for the oven slot
                if (orderController.currentlyMade[firstInBacklog][macTypeIndex] == 0)
                {
                    Instantiate(vanPan);
                }
                else if (orderController.currentlyMade[firstInBacklog][macTypeIndex] == 1)
                {
                    Instantiate(chocPan);
                }
                else if (orderController.currentlyMade[firstInBacklog][macTypeIndex] == 2)
                {
                    Instantiate(strawPan);
                }
                else
                {
                    Instantiate(burntPan);
                }

                orderController.currentlyMade[firstInBacklog][screenIndex] = slotScreenNum; //change the screen # of object going into slot
            }
        }
    }

    public int CheckScreenNum (int screenNum) //returns the index of the first order on a specific screen
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
