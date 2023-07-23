using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowButtons : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;

    [SerializeField] float moveAmount = 19;

    [SerializeField] float firstScreenX = 0;
    [SerializeField] float lastScreenX = 57;

    int arrowState = 1; // 0 = all arrows are visible, 1 = must re-enable left arrow, 2 = must re-enable right arrow

    public void moveCamRight()
    {
        camTransform.position += new Vector3 (moveAmount, 0);

        if (arrowState == 1)
        {
            leftArrow.SetActive(true);
            arrowState = 0;
        }

        if (camTransform.position.x >= lastScreenX)
        {
            //disable right arrow
            rightArrow.SetActive(false);
            arrowState = 2;
        }
    }

    public void moveCamLeft()
    {
        camTransform.position -= new Vector3(moveAmount, 0);

        if (arrowState == 2)
        {
            rightArrow.SetActive(true);
            arrowState = 0;
        }

        if (camTransform.position.x <= firstScreenX)
        {
            //disable left arrow
            leftArrow.SetActive(false);
            arrowState = 1;
        }
    }

























}
