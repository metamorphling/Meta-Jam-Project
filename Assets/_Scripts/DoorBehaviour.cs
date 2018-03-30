using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    public Sprite doorOpened;
    public GameObject doorLight;
    public SpriteRenderer doorDark;
	
	public void OpenDoor () {
        doorDark.sprite = doorOpened;
        doorLight.GetComponent<SpriteRenderer>().enabled = true;
    }
}
