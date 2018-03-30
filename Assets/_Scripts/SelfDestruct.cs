using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public List<AudioClip> shoots;


    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

	void Start () {
        int rand = Random.Range(0,3);
        GetComponent<AudioSource>().clip = shoots[rand];
        GetComponent<AudioSource>().Play();
        StartCoroutine("SelfDestroy");
	}

}
