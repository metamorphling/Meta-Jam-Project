using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

	void Start () {
        StartCoroutine("SelfDestroy");
	}

}
