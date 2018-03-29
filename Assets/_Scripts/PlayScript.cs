using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour {

	void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(StartPlay);
    }

    void StartPlay()
    {
        Debug.Log("looking for bomb");
        GameObject bomb = GameObject.FindGameObjectWithTag("Bomb");
        if (bomb != null)
        {
            Debug.Log("found a bomb");
            bomb.GetComponent<BombScript>().StartBlow();
        }
    }
}
