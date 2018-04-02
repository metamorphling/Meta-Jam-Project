using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour {

    Animator anim;

    void Start () {
        anim = GameObject.Find("Glitch").GetComponent<Animator>();
        anim.SetBool("loopGlitch", true);
    }
	
	void Update () {
		
	}
}
