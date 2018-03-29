using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour {

    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
	}

    IEnumerator StartBreaking()
    {
        gameObject.layer = LayerMask.NameToLayer("UI");
        anim.enabled = true;
        anim.SetTrigger("break");
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
	
    public void Break()
    {
        StartCoroutine("StartBreaking");
    }
}
