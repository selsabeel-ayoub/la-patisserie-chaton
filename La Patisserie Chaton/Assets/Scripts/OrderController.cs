using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class OrderController : MonoBehaviour
{
    [SerializeField] private ObjectController objectController;

    [SerializeField] private int newOrderInterval = 30;

    [SerializeField] private int scoreMinTime = 30;
    [SerializeField] private int scoreMaxTime = 60;

    [SerializeField] private GameObject newOrderText;
    [SerializeField] private GameObject cantTakeOrderTxt;
    [SerializeField] private Button orderButton;
    [SerializeField] private Transform canvas;
    [SerializeField] private Text scoreText;
    [SerializeField] private Sprite filledStarSprite;
    [SerializeField] private Sprite emptyStarSprite;
    [SerializeField] private Animator sellCatAnim;
    [SerializeField] private PlayableDirector sellingTimeline;

    [SerializeField] private Transform[] orderSlots;
    [SerializeField] private SpriteRenderer[] starSprites;

    [SerializeField] private Transform catsParent;
    [SerializeField] private GameObject calicoCat;
    [SerializeField] private GameObject gingerCat;
    [SerializeField] private GameObject greyCat;


    private GameObject canvasPanel;
    private GameObject cremPan;
    private GameObject catPrefab;
    private GameObject catObj;

    //2D Lists
    public List<List<int>> newOrders = new List<List<int>>(); // [cat, cookie type, cream type]
    public List<List<int>> takenOrders = new List<List<int>>(); // [cat, cookie type, cream type, time taken, time sold]
    public List<List<int>> currentlyMade = new List<List<int>>(); // [screen, cookie type, cream type]

    private List<int> selectedOrder = new List<int>();


    [HideInInspector] public int screenIndex = 0;
    [HideInInspector] public int cookieTypeIndex_made = 1;
    [HideInInspector] public int creamTypeIndex_made = 2;

    //used for both newOrders[] and takenOrders[]
    [HideInInspector] public int catTypeIndex = 0;
    [HideInInspector] public int cookieTypeIndex = 1;
    [HideInInspector] public int creamTypeIndex = 2;
    private int timeTakenIndex = 3;
    private int timeSoldIndex = 4;

    public List<Transform> nextCats = new List<Transform>();
    private Vector3 catPos = new Vector3(0, 0, 0);
    private float catDist = -3.8f;

    private int totalStars = 0;
    private int stars = 0;

    private int flavourNum = 3;
    private int catTypeNum = 3;

    private int newOrderTextWait = 2;
    private int cantTakeOrderTextWait = 1;
    private int newOrderIndex = 0;
    private int selectedOrderNum;
    private int slotIndex;
    private int slotAmt = 9;


    private bool newOrderFncExecuted = false;
    [HideInInspector] public bool canTakeOrder;

    private void Start()
    {
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
    private void NewOrder()
    {
        if (nextCats.Count < 8)
        {
            int catType = Random.Range(0, catTypeNum); // 0 = calico, 1 = ginger, 2 = grey
            int cookieType = Random.Range(0, flavourNum); // 0 = vanilla, 1 = choc, 2 = strawb
            int creamType = Random.Range(0, flavourNum); // 0 = vanilla, 1 = choc, 2 = strawb

            newOrders.Add(new List<int>());
            newOrders[newOrderIndex].Add(catType);
            newOrders[newOrderIndex].Add(cookieType);
            newOrders[newOrderIndex].Add(creamType);

            // Debug.Log(newOrderIndex + "cookie type: " + newOrders[newOrderIndex][cookieTypeIndex] + "cream type:" + newOrders[newOrderIndex][creamTypeIndex]);
            newOrderIndex++;

            if (Time.time != 0)
            {
                StartCoroutine(newOrderUICoroutine());
            }


            if (catType == 0)
            {
                catPrefab = calicoCat;
            }
            else if (catType == 1)
            {
                catPrefab = gingerCat;
            }
            else
            {
                catPrefab = greyCat;
            }

            catPos.x = catDist * (nextCats.Count);
            catPos.y = -1;
            catObj = Instantiate(catPrefab, catPos, Quaternion.identity, catsParent);

            nextCats.Add(catObj.transform);

            if (catObj.transform.GetSiblingIndex() == 0)
            {
                catObj.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    private IEnumerator newOrderUICoroutine ()
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

            int item1 = newOrders[0][catTypeIndex]; //cat type
            int item2 = newOrders[0][cookieTypeIndex]; //cookie type
            int item3 = newOrders[0][creamTypeIndex]; //cream type

            int listLen = takenOrders.Count;

            takenOrders.Add(new List<int>());

            takenOrders[listLen].Add(item1);
            takenOrders[listLen].Add(item2); 
            takenOrders[listLen].Add(item3);
            takenOrders[listLen].Add((int)Time.time);

            newOrders.RemoveAt(0);
            newOrderIndex -= 1;

            // Debug.Log("cookie type: " + takenOrders[listLen][cookieTypeIndex] + " cream type: " + takenOrders[listLen][creamTypeIndex]);

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
            StartCoroutine(cantTakeOrderCoroutine());
        }
    }

    private IEnumerator cantTakeOrderCoroutine()
    {
        GameObject cantTakeOrderTxtObj = Instantiate(cantTakeOrderTxt, canvas);
        yield return new WaitForSeconds(cantTakeOrderTextWait);
        Destroy(cantTakeOrderTxtObj);
    }

    [ContextMenu("Sell Order")]
    public void SellOrder ()
    {
        for (int i = 0; i < 5; i++) // reset star sprites
        {
            starSprites[i].sprite = emptyStarSprite;
        }

        objectController.ResetCream();

        selectedOrderNum = OrderSelection.selectedOrderNum;

        if (selectedOrderNum >= 0 && currentlyMade[0].Count == 3 && objectController.assemblyAnimDone) 
        {
            objectController.assemblyAnimDone = false;

            takenOrders[selectedOrderNum].Add((int)Time.time); //adding the current time as the "time sold" for the order

            sellCatAnim.SetInteger("catType", takenOrders[selectedOrderNum][catTypeIndex]) ; //set the sell timeline cat to the correct type


            selectedOrder = takenOrders[selectedOrderNum]; //this is to compare currentlyMade to it when scoring
            takenOrders.RemoveAt(selectedOrderNum);
            Destroy(orderSlots[selectedOrderNum].transform.GetChild(0).gameObject);


            if ((selectedOrderNum != slotAmt - 1) && (orderSlots[selectedOrderNum + 1].childCount > 0)) // if the UI slot after the sold slot is not empty + it is not the last UI slot
            {
                for (int i = selectedOrderNum + 1; i <= slotAmt; i++)
                {
                    if (orderSlots[i].childCount > 0) //if UI slot after is being used
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


            Scoring();
            objectController.NextCreamSlot();

            sellingTimeline.Play();

            cremPan = GameObject.FindWithTag("cremPan");
            Destroy(cremPan);
        }
    }

    private void Scoring()
    {
        if (selectedOrder[cookieTypeIndex] == currentlyMade[0][cookieTypeIndex_made]) //Check if cookie type is the same
         {
            stars++;
            Debug.Log("correct cookie flavour");
         }

        if (selectedOrder[creamTypeIndex] == currentlyMade[0][creamTypeIndex_made]) //Check if cream type is the same
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
        }

        totalStars += stars;
        stars = 0;

        scoreText.text = totalStars.ToString();

        selectedOrder = new List<int>(); //reset selectedOrder
        currentlyMade.RemoveAt(0); // delete the most current currently made
    }
}
