using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlideOut : MonoBehaviour {

    public float lerpTime;
    public RectTransform playButton;

    bool sliding = false;
    bool play = false;
    float currentLerpTime = 0;
    Vector3 origPos;
    Vector3 startPos;
    Vector3 endPos;
    float inventorySize;
    float playSize;
    RectTransform inventory;

    enum Status
    {
        Playing,
        Editing,
        Hiding
    }

    Status animationStatus = Status.Hiding;

    void Start() {
        /* depending on screen size have to scale game objects positions accordingly */
        CanvasScaler ScreenScale = GetComponentInParent<CanvasScaler>();
        inventory = GetComponent<RectTransform>();
        /* hardcoded usage of 90% of width so that slide bar won't slide completely */
        inventorySize = (inventory.sizeDelta.x / (ScreenScale.referenceResolution.x / Screen.width) * 0.9f);
        playSize = (playButton.sizeDelta.x / (ScreenScale.referenceResolution.x / Screen.width));
        startPos = transform.position;
        origPos = startPos;
        endPos = transform.position - transform.right * inventorySize;
        Debug.Log(playSize  + " play " + playButton.sizeDelta.x);
    }

    public void StartSlideOut()
    {
        currentLerpTime = 0;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
        startPos = transform.position;
        endPos = origPos - transform.right * inventorySize;
        animationStatus = Status.Editing;
        sliding = true;
    }

    IEnumerator PlayToEdit()
    {
        yield return new WaitForSeconds(0.6f);
        Animator anim = transform.GetChild(0).GetComponent<Animator>();
        anim.SetTrigger("change");
    }

    public void PressPlay()
    {
        startPos = transform.position;
        currentLerpTime = 0;
        if (animationStatus == Status.Editing)
        {
            animationStatus = Status.Playing;
            StartCoroutine("PlayToEdit");
            endPos = origPos - transform.right * playSize * 1.1f;
        } else if (animationStatus == Status.Playing)
        {
            animationStatus = Status.Hiding;
            endPos = origPos;
        }
        sliding = true;
    }

    void Sliding()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, perc);

        if (perc == 1)
        {
            sliding = false;
        }
    }
    void Update() {
        if (sliding == false)
            return;
        Sliding();
    }
}
