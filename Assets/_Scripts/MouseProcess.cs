using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseProcess : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform gameSystemBG;
    public InventorySlideOut inventory;
    Transform cartridges;

    /* mouse over variables */
    public float lerpTime;
    public float moveDistanceY;
    public float moveDistanceX;

    Vector3 startPos;
    Vector3 endPos;
    Vector3 origPos;
    float currentLerpTime;

    /* mouse clicked variables */
    public float arcLerpTime;
    public float arcMoveDistanceY;
    public float arcMoveDistanceX;

    /* move into game system variables */
    public float insertLerpTime;
    public Transform insertPlace;

    /* screen scale helper */
    float scaler;

    /* parenting to overlay when flying */
    Transform origParent;

    enum CartridgeStatus
    {
        MouseOver,
        Arc,
        Insert,
        Inserted,
        None
    }

    bool modePlay = false;
    int siblingIndex; 
    CartridgeStatus animationStatus = CartridgeStatus.None;
    bool goBack = false;

    List<Vector3> lastPositions;

    void Start() {
        lastPositions = new List<Vector3>();
        origPos = transform.position;
        cartridges = transform.parent.transform;
        CanvasScaler ScreenScale = GetComponentInParent<CanvasScaler>();
        scaler = ScreenScale.referenceResolution.x / Screen.width;
        moveDistanceX = (moveDistanceX / scaler);
        moveDistanceY = (moveDistanceY / scaler);
        siblingIndex = transform.GetSiblingIndex();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationStatus == CartridgeStatus.None)
        {
            startPos = origPos;
            endPos = transform.position + transform.up * moveDistanceY + transform.right * moveDistanceX;
            currentLerpTime = 0f;
            animationStatus = CartridgeStatus.MouseOver;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationStatus == CartridgeStatus.MouseOver)
        {
            endPos = origPos;
            startPos = transform.position;
            currentLerpTime = 0f;
        }
    }

    public void ChooseCartridge()
    {
        lastPositions.Add(origPos);
        //lastPositions.Add(transform.position);
        animationStatus = CartridgeStatus.Arc;
        currentLerpTime = 0;
        startPos = transform.position;
        endPos = transform.position + transform.up * arcMoveDistanceY + transform.right * (insertPlace.position.x - transform.position.x);
    }

    void PopUpProcessing()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, perc);

        if (perc == 1 && transform.position == origPos)
        {
            animationStatus = CartridgeStatus.None;
            if (goBack)
            {

            }
        }
    }

    void CartridgeArcProcessing()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        perc = Mathf.Sin(Mathf.PI * perc * 0.5f);
        if (goBack == false)
        {
            float percAxisY = (transform.position.y - startPos.y) / (endPos.y - startPos.y);
            percAxisY = Mathf.Sin(Mathf.PI * percAxisY);
            transform.position = Vector3.Lerp(startPos + (-transform.right) * percAxisY * 20, endPos + (-transform.right) * percAxisY * 20, perc);
        }
        else
        {
            float percAxisY = Mathf.Abs((transform.position.y - startPos.y) / (endPos.y - startPos.y));
            percAxisY = Mathf.Sin(Mathf.PI * percAxisY);
            transform.position = Vector3.Lerp(startPos + (-transform.right) * percAxisY * 20, endPos + (-transform.right) * percAxisY * 20, perc);
        }


        if (perc == 1)
        {
            currentLerpTime = 0;
            startPos = transform.position;
            if (goBack == false)
            {
                transform.SetParent(gameSystemBG);
                endPos = insertPlace.position;
                animationStatus = CartridgeStatus.Insert;
                lastPositions.Add(transform.position);
            } else
            {
                animationStatus = CartridgeStatus.None;
            }
        }
    }

    void CartridgeInsertProcessing()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > insertLerpTime)
        {
            currentLerpTime = insertLerpTime;
        }

        float perc = currentLerpTime / insertLerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, perc);

        if (perc == 1)
        {
            if (goBack == false)
            {
                inventory.StartSlideOut();
                animationStatus = CartridgeStatus.Inserted;
            } else
            {
                currentLerpTime = 0;
                startPos = transform.position;
                endPos = lastPositions[lastPositions.Count - 1];
                lastPositions.RemoveAt(lastPositions.Count - 1);
                animationStatus = CartridgeStatus.Arc;
                transform.SetParent(cartridges);
                transform.SetSiblingIndex(siblingIndex);
            }
        }
    }

    public void ReplayClick()
    {
        if (animationStatus != CartridgeStatus.Inserted)
            return;

        if (modePlay == false)
        {
            modePlay = true;
            return;
        }
        modePlay = false;

        goBack = true;
    }

    private void Update()
    {
        if (animationStatus == CartridgeStatus.MouseOver)
        {
            PopUpProcessing();
        }
        else if (animationStatus == CartridgeStatus.Arc)
        {
            CartridgeArcProcessing();
        }
        else if (animationStatus == CartridgeStatus.Insert)
        {
            CartridgeInsertProcessing();
        }
        else if(animationStatus == CartridgeStatus.None)
        {
        }
        else if (animationStatus == CartridgeStatus.Inserted)
        {
            if (goBack == true)
            {
                currentLerpTime = 0;
                startPos = transform.position;
                endPos = lastPositions[lastPositions.Count - 1];
                lastPositions.RemoveAt(lastPositions.Count - 1);
                animationStatus = CartridgeStatus.Insert;
            }
        }
    }
}
