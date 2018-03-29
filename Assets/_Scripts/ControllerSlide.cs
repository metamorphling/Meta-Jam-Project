using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerSlide : MonoBehaviour {
    public float lerpTime;

    float currentLerpTime = 0;
    Vector3 origPos;
    Vector3 origEndPos;
    Vector3 startPos;
    Vector3 endPos;
    bool sliding = false;
    float controllerSize;
    bool isPlaying = false;

    private void Start()
    {
        RectTransform t = GetComponent<RectTransform>();
        CanvasScaler ScreenScale = GetComponentInParent<CanvasScaler>();
        controllerSize = (t.sizeDelta.y / (ScreenScale.referenceResolution.y / Screen.height));
        origPos = transform.position;
        origEndPos = transform.position + Vector3.up * controllerSize;
    }

    IEnumerator TimedSliding()
    {
        yield return new WaitForSeconds(1f);
        if (sliding == false)
            sliding = true;
    }

    public void StartSlideTimer()
    {
        currentLerpTime = 0;

        if (isPlaying == true || sliding == true)
        {
            startPos = transform.position;
            endPos = origPos;
        }
        else
        {
            startPos = transform.position;
            endPos = origEndPos;
        }
        StartCoroutine("TimedSliding");
    }

    IEnumerator RestartLevel()
    {
        GameObject gl = GameObject.Find("Glitch");
        gl.GetComponent<Animator>().SetTrigger("playGlitch");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            {
                isPlaying = false;
                StartCoroutine("RestartLevel");
            }
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
