using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class OrderController : MonoBehaviour
{
    OrderSelection orderSelection;

    [SerializeField] int newOrderInterval = 30;


    [SerializeField] int scoreMinTime = 30;
    [SerializeField] int scoreMaxTime = 60;


    [SerializeField] GameObject newOrderText;
    [SerializeField] Button orderButton;
    [SerializeField] Transform canvas;
    [SerializeField] Text scoreText;
    [SerializeField] Sprite filledStarSprite;
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] PlayableDirector sellingTimeline;

    //    [SerializeField] BoxCollider2D[] creamColliders;


    GameObject canvasPanel;
    [SerializeField] Transform[] orderSlots;
    [SerializeField] SpriteRenderer[] starSprites;

    //2D Lists
    List<List<int>> newOrders = new List<List<int>>(); // [cookie type, cream type]
    public List<List<int>> takenOrders = new List<List<int>>(); // [screen, cookie type, cream type, time taken, time sold]
    public List<List<int>> currentlyMade = new List<List<int>>(); // [cookie type, cream type]

    List<int> selectedOrder = new List<int>();

    int totalStars = 0;
    int stars = 0;


    [HideInInspector] public int screenIndex = 0;
    [HideInInspector] public int cookieTypeIndex = 1;
    [HideInInspector] public int creamTypeIndex = 2;
    int timeTakenIndex = 3;
    int timeSoldIndex = 4;


    int flavourNum = 3;

    int newOrderTextWait = 2;
    int newOrderIndex = 0;
    int selectedOrderNum;
    int slotIndex;
    int slotAmt = 9;

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

    //Orders that have not been taken yet
    [ContextMenu("Add New Order")]
    void NewOrder()
    {
        int cookieType = Random.Range(0, flavourNum); // 0 = vanilla, 1 = choc, 2 = strawb
        int creamType = Random.Range(0, flavourNum); // 0 = vanilla, 1 = choc, 2 = strawb

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

            takenOrders[listLen].Add(0); // screen
            takenOrders[listLen].Add(item1); 
            takenOrders[listLen].Add(item2);
            takenOrders[listLen].Add((int)Time.time);

            newOrders.RemoveAt(0);
            newOrderIndex -= 1;

            Debug.Log("cookie type: " + takenOrders[listLen][cookieTypeIndex] + " cream type:" + takenOrders[listLen][creamTypeIndex]);

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
    public void SellOrder ()
    {
        for (int i = 0; i < 5; i++) // reset star sprites
        {
            starSprites[i].sprite = emptyStarSprite;
        }

        selectedOrderNum = OrderSelection.selectedOrderNum;

        if (selectedOrderNum >= 0)
        {
            takenOrders[selectedOrderNum].Add((int)Time.time); //adding the current time as the "time sold" for the order


            /*for (int x = 0; x < flavourNum; x++) //disable all the cream colliders 
            {
                creamColliders[x].enabled = false;
                //reenable when timelines over
            }*/

            selectedOrder = takenOrders[selectedOrderNum]; //this is to compare currentlyMade to it when scoring
            takenOrders.RemoveAt(selectedOrderNum);

            if (orderSlots[selectedOrderNum + 1].childCount > 0) // if the slot after the sold slot is not empty
            {
                for (int i = selectedOrderNum + 1; i <= slotAmt; i++)
                {
                    if (orderSlots[i].childCount > 0) //if slot it being used
                    {
                        orderSlots[i].GetChild(0).position = orderSlots[i - 1].position; //Move position

                        orderSlots[i].GetChild(0).SetParent(orderSlots[i - 1]); //Set parent
                    }
                    else //if any slot after is empty, stop checking if slots are empty/full 
                    {
                        break;
                    }
                }
            }

            Destroy(orderSlots[selectedOrderNum].transform.GetChild(0).gameObject);

            Scoring();

            sellingTimeline.Play();
        }
    }

    void Scoring()
    {
        if (selectedOrder[cookieTypeIndex] == currentlyMade[0][0]) //Check if cookie type is the same
         {
            stars++;
            Debug.Log("correct cookie flavour");
         }

        if (selectedOrder[creamTypeIndex] == currentlyMade[0][1]) //Check if cream type is the same
         {
             stars ++;
            Debug.Log("correct cream flavour");
        }

        if ((selectedOrder[timeSoldIndex] - selectedOrder[timeTakenIndex]) < scoreMinTime)
        {
            stars += 3; 
        }
        else if ((selectedOrder[timeSoldIndex] - selectedOrder[timeTakenIndex]) > scoreMaxTime)
        {
            stars ++; 
        }
        else
        {
            stars += 2;
        }

        for (int i = 0; i < stars; i++)
        {
            starSprites[i].sprite = filledStarSprite;
            //remember to reset all sprites to unfilled after
        }

        totalStars += stars;
        stars = 0;

        scoreText.text = totalStars.ToString();

        selectedOrder = new List<int>(); //reset selectedOrder
        currentlyMade.RemoveAt(0); // delete the most current currently made
    }
}
