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
    Transform canvasPanelTransform;

    Vector2 orderUIPosition = new Vector2(50, 330);

    //2D Lists
    List<List<int>> newOrders = new List<List<int>>();
    public List<List<int>> takenOrders = new List<List<int>>(); // [cookie type, cream type, time taken, time sold]
    List<List<int>> currentlyMade = new List<List<int>>();

    List<int> selectedOrder = new List<int>();

    int newOrderIndex = 0;
    int selectedOrderNum;

    bool newOrderFncExecuted = false;



    private void Start()
    {
        canvasPanel = GameObject.FindWithTag("orderPanel");
        canvasPanelTransform = canvasPanel.GetComponent<Transform>();

        orderSelection = GetComponent<OrderSelection>();
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
        if (newOrders.Count > 0)
        {
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

            Instantiate(orderButton, orderUIPosition, Quaternion.identity, canvasPanelTransform); //(object, pos, roation, parent)
            orderUIPosition += new Vector2(60, 0);
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
            Destroy(canvasPanel.transform.GetChild(selectedOrderNum).gameObject);

            //remember to also reset/delete "CurrentlyMade"

            //do scoring function()

            //move all the ui after it too sigh
        }
    }

    //when done scoring must reset selectedOrder list

}
