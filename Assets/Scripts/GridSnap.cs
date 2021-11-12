using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridSnap : MonoBehaviour
{
    private GridLayoutGroup mGrid;
    private RectTransform mRect;
    private ScrollRect mScrollRect;
    public float activeScale = 1;
    public float inactiveScale = 0.75f;
    public float stopVelocity = 800f;
    public float scaleTime = 15f;


    private Vector2 mTargetPos;
    private bool mDone = false;
    private float mT = 0;

    void Start()
    {
        mGrid = GetComponent<GridLayoutGroup>();
        mRect = GetComponent<RectTransform>();
        mScrollRect = GetComponentInParent<ScrollRect>();

        // auto adjust the width of the grid to have space for all the childs
        mRect.sizeDelta = new Vector2((transform.childCount + 2f) * mGrid.cellSize.x + (transform.childCount - 1f) * mGrid.spacing.x, mRect.sizeDelta.y);
        mTargetPos = mScrollRect.GetComponent<RectTransform>().anchoredPosition;
        mDone = false;
    }

    public void Update()
    {
        mT = Time.deltaTime * scaleTime;
        if (mT > 1f)
        {
            mT = 1f;
        }
        if (Mathf.Abs(mScrollRect.velocity.x) > stopVelocity && !mDone)
        {
            TouchUp();
        }
        if (!mDone && Mathf.Abs(mScrollRect.velocity.x) < stopVelocity)
        {
            mRect.localPosition = Vector2.Lerp(mRect.localPosition, mTargetPos, mT);
            if (Vector3.Distance(mRect.localPosition, mTargetPos) < 0.001f)
            {
                mRect.localPosition = mTargetPos;
                mDone = true;
            }
        }

        Vector2 tempPos = new Vector2(Mathf.Round(mRect.localPosition.x / (mGrid.cellSize.x + mGrid.spacing.x)) * (mGrid.cellSize.x + mGrid.spacing.x) * -1f, 0);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (Math.Abs(child.localPosition.x - tempPos.x) < 0.1f)
            {
                // do what you want with the child
                child.localScale = Vector3.Lerp(child.localScale, new Vector3(activeScale, activeScale, 1f), mT);
            }
            else
            {
                child.localScale = Vector3.Lerp(child.localScale, new Vector3(inactiveScale, inactiveScale, 1f), mT);
            }
        }
    }

    public void TouchDown()
    {
        mDone = true;
    }

    public void TouchUp()
    {
        float newX = Mathf.Round(mRect.localPosition.x / (mGrid.cellSize.x + mGrid.spacing.x)) * (mGrid.cellSize.x + mGrid.spacing.x);
        newX = Mathf.Sign(newX) * Mathf.Min(Mathf.Abs(newX), (mRect.rect.width - mScrollRect.GetComponent<RectTransform>().rect.width) / 2f);
        mTargetPos = new Vector2(newX, 0);
        mDone = false;
    }
}
