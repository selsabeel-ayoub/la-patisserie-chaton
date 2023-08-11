using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SellOnClick : MonoBehaviour
{
    OrderController orderController;
    AudioSource bellAudio;

    private void Start()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
        bellAudio = GetComponent<AudioSource>();
    }
    private void OnMouseDown()
    {
        if (orderController.SellOrder())
        {
            bellAudio.Play();
        }
    }
}
