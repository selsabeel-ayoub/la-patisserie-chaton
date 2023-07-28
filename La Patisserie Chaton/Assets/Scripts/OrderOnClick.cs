using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderOnClick : MonoBehaviour
{
    OrderController orderController;
    Animator catAnimator;

    [SerializeField] int animWait = 2;

    private void Awake()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
        catAnimator = GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        orderController.TakeOrder();
        if (orderController.canTakeOrder == true)
        {
            StartCoroutine(AnimCoroutine());
        }
    }

    IEnumerator AnimCoroutine ()
    {
        catAnimator.SetBool("isTalking", true);
        yield return new WaitForSeconds(animWait);
        catAnimator.SetBool("isTalking", false);
    }
}
