using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowButtons : MonoBehaviour
{
    public int firstPage = 0;
    public int lastPage = 3;

    [SerializeField] Transform camTransform;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;

    [SerializeField] float moveAmount = 19;

    int currentPage = 0;


    private void Start()
    {
        updateArrows();
    }
    public void moveCamRight()
    {
        if (isNotLastPage())
        {
            camTransform.position += new Vector3(moveAmount, 0);
            currentPage += 1;
        }
        updateArrows();
    }

    public void moveCamLeft()
    {
        if (isNotFirstPage())
        {
            camTransform.position -= new Vector3(moveAmount, 0);
            currentPage -= 1;
        }
        updateArrows();
    }


    public void updateArrows()
    {
        leftArrow.SetActive(isNotFirstPage());
        rightArrow.SetActive(isNotLastPage());
    }

    bool isNotFirstPage()
    {
        return currentPage > firstPage;
    }

    bool isNotLastPage()
    {
        return currentPage < lastPage;
    }
}
