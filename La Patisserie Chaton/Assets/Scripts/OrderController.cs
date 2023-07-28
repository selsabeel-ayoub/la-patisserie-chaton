using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class OrderController : MonoBehaviour
{
    OrderSelection orderSelection;

    [SerializeField] int newOrderInterval = 30;
    [SerializeField] int newOrderTextWait = 2;

    [SerializeField] GameObject newOrderText;
    [SerializeField] Button orderButton;
    [SerializeField] Transform canvas;

    GameObject canvasPanel;
    [SerializeField] Transform[] orderSlots;

    //2D Lists
    List<List<int>> newOrders = new List<List<int>>();
    public List<List<int>> takenOrders = new List<List<int>>(); // [cookie type, cream type, time taken, time sold]
    List<List<int>> currentlyMade = new List<List<int>>();

    List<int> selectedOrder = new List<int>();

    int newOrderIndex = 0;
    int selectedOrderNum;
    int slotIndex;
    int slotAmt = 10;

    bool newOrderFncExecuted = false;
    [HideInInspector] public bool canTakeOrder;



    private void Start()
    {
        orderSelection = GetComponent<OrderSelection>();
        canvasPanel = GameObject.FindWithTag("orderPanel");
    }

    private void Update()
    {
        if (((int)Time.time % newOrderInterval == 0) && (newOrderFncExecuted == false))
        {
            newOrderFncExecuted = true;
            NewOrder();
        }
        else if ((int)Time.time % newOrderInterval != 0)
        {
            newOrderFncExecuted = false;
        }
    }

    //These are orders that have not been taken yet
    [ContextMenu("Add New Order")]
    void NewOrder() // call this funcion in update of fixed update after a certain amount of time
    {

        int cookieType = Random.Range(1, 4); // 1 = vanilla, 2 = choc, 3 = strawb
        int creamType = Random.Range(1, 4); // 1 = vanilla, 2 = choc, 3 = strawb

        newOrders.Add(new List<int>());
        newOrders[newOrderIndex].Add(cookieType);
        newOrders[newOrderIndex].Add(creamType);

        Debug.Log(newOrderIndex + "cookie type: " + newOrders[newOrderIndex][0] + "cream type:" + newOrders[newOrderIndex][1]);
        newOrderIndex++;

        if (Time.time != 0)
        {
            StartCoroutine(newOrderUICoroutine());
        }
    }

    IEnumerator newOrderUICoroutine ()
    {
        GameObject newOrderTextObj = Instantiate(newOrderText, canvas);
        yield return new WaitForSeconds(newOrderTextWait);
        Destroy(newOrderTextObj);
    }

    [ContextMenu("Take Order")]
    public void TakeOrder ()
    {
        if ((newOrders.Count > 0) && (takenOrders.Count < slotAmt))
        {
            canTakeOrder = true;

            int item1 = newOrders[0][0]; //cookie type
            int item2 = newOrders[0][1]; //cream type

            int listLen = takenOrders.Count;

            takenOrders.Add(new List<int>());

            takenOrders[listLen].Add(item1);
            takenOrders[listLen].Add(item2);
            takenOrders[listLen].Add((int)Time.time);

            newOrders.RemoveAt(0);
            newOrderIndex -= 1;

            Debug.Log("cookie type: " + takenOrders[listLen][0] + " cream type:" + takenOrders[listLen][1]);

            for (slotIndex = 0; slotIndex <= slotAmt; slotIndex++)
            {
                if (orderSlots[slotIndex].transform.childCount <= 0)
                {
                    break;
                }
            }

            Instantiate(orderButton, orderSlots[slotIndex].position, Quaternion.identity, orderSlots[slotIndex]); //(object, pos, roation, parent)
        }
        else
        {
            canTakeOrder = false;
        }
    }

    [ContextMenu("Sell Order")]
    void SellOrder ()
    {

        selectedOrderNum = OrderSelection.selectedOrderNum;

        if (selectedOrderNum >= 0)
        {
            takenOrders[selectedOrderNum].Add((int)Time.time); //adding the current time as the "time sold" for the order

            //play a timeline

            selectedOrder = takenOrders[selectedOrderNum]; //this is to compare currentlyMade to it when scoring
            takenOrders.RemoveAt(selectedOrderNum);

            Destroy(orderSlots[selectedOrderNum].transform.GetChild(0).gameObject);

            //move all the ui after it too sigh
            //check slots after orderslots[selectedordernum] w a loop and if they have a child, set their parents to the slot before, and move them


            //remember to also reset/delete "CurrentlyMade"

            //do scoring function()

        }
    }

    //when done scoring must reset selectedOrder list

}
