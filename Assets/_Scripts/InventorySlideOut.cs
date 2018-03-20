using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlideOut : MonoBehaviour {

    public float lerpTime;
    public RectTransform inventory;

    bool sliding = false;
    float currentLerpTime = 0;
    Vector3 startPos;
    Vector3 endPos;
    void Start () {
        float inventorySize = inventory.rect.width;
        startPos = transform.position;
        endPos = transform.position - transform.right * (inventory.offsetMin.x + inventorySize);
    }

    public void StartSlideOut()
    {
        sliding = true;
    }

	void Update () {
        if (sliding == false)
            return;

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, perc);

        if (perc == 1)
            sliding = false;
    }
}
