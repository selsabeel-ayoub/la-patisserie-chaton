using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FlavourBowls : MonoBehaviour
{
    OrderController orderController;
    ObjectController objectController;

    [SerializeField] private int flavourType;
    [SerializeField] private Animator mixBowlAnimator;
    [SerializeField] private PlayableDirector mixBowlToPiping;
    [SerializeField] private GameObject pipingObj;

    [SerializeField] private SpriteRenderer pipingSpRenderer;
    [SerializeField] private Sprite vanPiping;
    [SerializeField] private Sprite chocPiping;
    [SerializeField] private Sprite strawbPiping;

    [SerializeField] private AudioSource pourAudio;
    [SerializeField] private AudioSource mixAudio;

    private int currentIndex = 0;
    private float mixAnimLen = 3f;
    private float mixPlayableLen = 2f;

    private void Start()
    {
        orderController = GameObject.FindWithTag("orderController").GetComponent<OrderController>();
        objectController = GameObject.FindWithTag("objectController").GetComponent<ObjectController>();
    }

    private void OnMouseDown()
    {
        currentIndex = orderController.currentlyMade.Count;

        orderController.currentlyMade.Add(new List<int>());
        orderController.currentlyMade[currentIndex].Add(0); // screen
        orderController.currentlyMade[currentIndex].Add(flavourType);

        // Debug.Log("cookie flavour chosen: " + orderController.currentlyMade[currentIndex][1]);

        mixBowlAnimator.SetInteger("flavor", flavourType);
        objectController.BowlsToPan();

        if (flavourType == 0)
        {
            pipingSpRenderer.sprite = vanPiping;
        }
        else if (flavourType == 1)
        {
            pipingSpRenderer.sprite = chocPiping;
        }
        else if (flavourType == 2)
        {
            pipingSpRenderer.sprite = strawbPiping;
        }

        StartCoroutine(waitForPlayable());
    }

    private IEnumerator waitForPlayable ()
    {
        pourAudio.Play();
        yield return new WaitForSeconds(1);
        mixAudio.Play();
        yield return new WaitForSeconds(mixAnimLen);
        mixBowlToPiping.Play();

        yield return new WaitForSeconds(mixPlayableLen);
        pipingObj.GetComponent<DragAndDrop>().enabled = true;
        mixBowlAnimator.SetInteger("flavor", -1);
    }

}
