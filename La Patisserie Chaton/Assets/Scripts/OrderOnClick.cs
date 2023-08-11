using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderOnClick : MonoBehaviour
{
    private OrderController orderController;
    private Animator catAnimator;
    private BoxCollider2D nextCatCollider;

    private bool orderTaken = false;
    private bool isCatSlide = false;
    private bool nextCatSlide = false;
    private float talkAnimWait = 1.2f;
    private float slideSpeed = 13;
    private float nextSlideSpeed = 6;
    private int catsLen;


    private void Awake()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
        catAnimator = GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        if (orderTaken == false) //this is so you cant take 2 orders from 1 cat
        {
            orderTaken = true;
            orderController.TakeOrder();
            if (orderController.canTakeOrder == true)
            {
                StartCoroutine(TalkCoroutine());
            }
            else
            {
                orderTaken = false;
            }
        }
    }

    private void FixedUpdate ()
    {
        catsLen = orderController.nextCats.Count;

        if (isCatSlide)
        {
            transform.position += new Vector3(slideSpeed, 0, 0) * Time.deltaTime;

            if (transform.position.x >= 11)
            {
                if (catsLen > 1)
                {
                    nextCatSlide = true;
                }
                else
                {
                    orderController.nextCats.RemoveAt(0);
                    Destroy(gameObject);
                }

                isCatSlide = false;
            }
        }

        if (nextCatSlide)
        {
            for (int x = 1; x <= catsLen -1; x++)
            {
                orderController.nextCats[x].position += new Vector3(nextSlideSpeed, 0, 0) * Time.deltaTime;
            }

            if (orderController.nextCats[1].position.x >= 0)
            {
                nextCatSlide = false;
                nextCatCollider = transform.parent.GetChild(1).gameObject.GetComponent<BoxCollider2D>();
                nextCatCollider.enabled = true;

                orderController.nextCats.RemoveAt(0);
                Destroy(gameObject);
            }
        }

    }

    IEnumerator TalkCoroutine ()
    {
        catAnimator.SetBool("isTalking", true);
        yield return new WaitForSeconds(talkAnimWait);
        catAnimator.SetBool("isTalking", false);
        isCatSlide = true;
    }
}
