using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideHelp : MonoBehaviour {

    public Image img;

    private void Update()
    {
        if (img.enabled == true && Input.GetButtonDown("Fire1"))
        {
            img.enabled = false;
        }
    }
}
