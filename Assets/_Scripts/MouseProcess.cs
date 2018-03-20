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

    enum CartridgeStatus
    {
        MouseOver,
        Arc,
        Insert,
        Finished
    }

    CartridgeStatus animationStatus = CartridgeStatus.Finished;

    void Start() {
        origPos = transform.position;
        cartridges = transform.parent.transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationStatus == CartridgeStatus.Finished)
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
            animationStatus = CartridgeStatus.Finished;
    }

    public void ChooseCartridge()
    {
        animationStatus = CartridgeStatus.Arc;
        currentLerpTime = 0;
        startPos = transform.position;
        endPos = transform.position + transform.up * arcMoveDistanceY + transform.right * (insertPlace.position.x - transform.position.x);
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
        //float percAxisX = (transform.position.x - startPos.x) / (endPos.x - startPos.x);
        //percAxisX = Mathf.Sin(Mathf.PI * percAxisX);
        float percAxisY = (transform.position.y - startPos.y) / (endPos.y - startPos.y);
        percAxisY = Mathf.Sin(Mathf.PI * percAxisY);

        transform.position = Vector3.Lerp(startPos + (-transform.right) * percAxisY * 20, endPos + (-transform.right) * percAxisY * 20, perc);

        if (perc == 1)
        {
            transform.SetParent(gameSystemBG);
            endPos = insertPlace.position;
            startPos = transform.position;
            currentLerpTime = 0;
            animationStatus = CartridgeStatus.Insert;
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
            animationStatus = CartridgeStatus.Finished;
            inventory.StartSlideOut();
        }
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
        else if(animationStatus == CartridgeStatus.Finished)
        {
        }
    }
}
