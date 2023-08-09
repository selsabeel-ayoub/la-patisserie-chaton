using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectController : MonoBehaviour
{
    [HideInInspector] public bool ovenEmpty = true;
    [HideInInspector] public bool assemblyAnimDone = false;
    [HideInInspector] public bool areMacsPiped = false;

    [SerializeField] private OrderController orderController;

    [SerializeField] private BoxCollider2D[] flavBowlColliders;
    [SerializeField] private BoxCollider2D[] cremColliders;
    [SerializeField] private BoxCollider2D panCollider;
    [SerializeField] private BoxCollider2D pipeCollider;
    [SerializeField] private Transform inOvenObj;

    [SerializeField] private PlayableDirector bowlsToPanPlayable;
    [SerializeField] private PlayableDirector panToOvenRoomPlayable;
    [SerializeField] private PlayableDirector panToCreamRoomPlayable;

    private float panToOvenRoomPlayableLen = 1.5f;

    //change later to just one gameobject with sprite changing (Gameobject ovenPan)
    [SerializeField] private GameObject vanPan_oven;
    [SerializeField] private GameObject chocPan_oven;
    [SerializeField] private GameObject strawPan_oven;
    [SerializeField] private GameObject vanPan_crem;
    [SerializeField] private GameObject chocPan_crem;
    [SerializeField] private GameObject strawPan_crem;
    [SerializeField] private GameObject burntPan_crem;

    [SerializeField] private GameObject vanCream;
    [SerializeField] private GameObject chocCream;
    [SerializeField] private GameObject strawCream;

    [SerializeField] private GameObject vanMac;
    [SerializeField] private GameObject chocMac;
    [SerializeField] private GameObject strawMac;

    private Transform ovenPan;
    private GameObject macaron;
    private Transform cremPan;
    private GameObject creams;
    private Animator topMacAnimator;

    private int ovenBacklogScreen = 1;
    private int ovenSlotScreen = 2;
    private int cremBacklogScreen = 4;
    private int cremSlotScreen = 5;

    /*Screens: 0 - mixing room
               1 - waiting for oven slot
               2 - in oven slot
               3 - in oven
               4 - waiting for cream slot
               5 - in cream slot */

    private int screenIndex;
    private int macTypeIndex;
    private int cremTypeIndex;
    private int n;

    private int macFlavorNum = 3;
    private int cremFlavorNum = 3;

    private float panToCreamRoomPlayableLen = 2f;


    private void Start()
    {
        screenIndex = orderController.screenIndex;
        macTypeIndex = orderController.cookieTypeIndex_made;
        cremTypeIndex = orderController.creamTypeIndex_made;
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

    public void PipeMacaron()
    {
        pipeCollider.enabled = false;

        n = orderController.currentlyMade.Count - 1;

        macaron =  InstantiateFlavor(vanMac, chocMac, strawMac, n, macTypeIndex);

        ovenPan = GameObject.FindWithTag("ovenPan").GetComponent<Transform>();
        macaron.transform.SetParent(ovenPan);
        areMacsPiped = true;
    }

    public void PanToOvenScreen ()
    {
        areMacsPiped = false;
        panCollider.enabled = false;
        panToOvenRoomPlayable.Play();

        for (int x = 0; x < macFlavorNum; x++)
        {
            flavBowlColliders[x].enabled = true;
        }

        orderController.currentlyMade[CheckScreenNum(0)][screenIndex] = 1; //change screen num (0 -> 1)
        NextInOvenSlot();

        StartCoroutine(waitForPanPlayable());
    }

    private IEnumerator waitForPanPlayable()
    {
        yield return new WaitForSeconds(panToOvenRoomPlayableLen);
        Destroy(macaron);
        pipeCollider.enabled = true;
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

    private IEnumerator ResetInOvenObj ()
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

        creams = InstantiateFlavor(vanCream, chocCream, strawCream, 0, cremTypeIndex);

        cremPan = GameObject.FindWithTag("cremPan").GetComponent<Transform>();
        creams.transform.SetParent(cremPan);

        for (int x = 0; x < 3; x++)
        {
            topMacAnimator = cremPan.GetChild(x).gameObject.GetComponent<Animator>();
            topMacAnimator.SetBool("isAssembling", true);
        }

        StartCoroutine(IsAssemblyAnimDone());
    }

    private IEnumerator IsAssemblyAnimDone ()
    {
        yield return new WaitForSeconds(1.9f);
        assemblyAnimDone = true;
    }

    public void ResetCream ()
    {
        for (int x = 0; x < cremFlavorNum; x++)
        {
            cremColliders[x].enabled = true;
        }
    }


    private GameObject InstantiateFlavor (GameObject vanilla, GameObject choc, GameObject strawb, int orderIndex, int typeIndex)
    {
        GameObject obj;

        if (orderController.currentlyMade[orderIndex][typeIndex] == 0)
        {
            obj = Instantiate(vanilla);
        }
        else if (orderController.currentlyMade[orderIndex][typeIndex] == 1)
        {
            obj = Instantiate(choc);
        }
        else
        {
            obj = Instantiate(strawb);
        }

        return obj;
    }

    private void NextInOvenSlot()
    {
        NextInSlot(ovenSlotScreen, ovenBacklogScreen, vanPan_oven, chocPan_oven, strawPan_oven, null);
    }

    public void NextCreamSlot()
    {
        NextInSlot(cremSlotScreen, cremBacklogScreen, vanPan_crem, chocPan_crem, strawPan_crem, burntPan_crem);
    }


    private void NextInSlot(int slotScreenNum, int backlogScreenNum, GameObject vanPan, GameObject chocPan, GameObject strawPan, GameObject burntPan)
    {
        if (CheckScreenNum(slotScreenNum) == -1) //only if there isnt anything in the slot right now
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
