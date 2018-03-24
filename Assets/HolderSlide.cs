using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HolderSlide : MonoBehaviour {
    public float lerpTime;

    float currentLerpTime = 0;
    Vector3 origPos;
    Vector3 startPos;
    Vector3 endPos;
    bool sliding = false;
    float holderSize;
    bool isPlaying = false;

    private void Start()
    {
        RectTransform t = GetComponent<RectTransform>();
        CanvasScaler ScreenScale = GetComponentInParent<CanvasScaler>();
        holderSize = (t.sizeDelta.y / (ScreenScale.referenceResolution.y / Screen.height));
        origPos = transform.position;
    }

    IEnumerator TimedSliding()
    {
        yield return new WaitForSeconds(0.2f);
        sliding = true;
    }

    public void StartSlideTimer()
    {
        currentLerpTime = 0;
        if (isPlaying == true)
        {
            startPos = transform.position;
            endPos = transform.position + Vector3.up * holderSize * 0.8f; //0.6 to slightly show
        }
        else
        {
            startPos = origPos;
            endPos = transform.position - Vector3.up * holderSize * 0.8f;
        }

        sliding = true;
        //StartCoroutine("TimedSliding");
    }
    
    void Sliding()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        perc = perc * perc * (3f - 2f * perc);
        transform.position = Vector3.Lerp(startPos, endPos, perc);

        if (perc == 1)
        {
            sliding = false;
            if (isPlaying == true)
                isPlaying = false;
            else
                isPlaying = true;
        }
    }
    void Update()
    {
        if (sliding == false)
            return;
        Sliding();
    }
}
