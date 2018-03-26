﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    CircleCollider2D coll;
    

    IEnumerator blow ()
    {
        yield return new WaitForSeconds(2f);
        Collider2D[] res = new Collider2D[20];
        ContactFilter2D filter = new ContactFilter2D();
        int limit = coll.OverlapCollider(filter, res);
        int counter = 1;
        foreach (Collider2D go in res)
        {
            if (go.gameObject && go.gameObject.tag == "Wall")
                Destroy(go.gameObject);
            if (counter++ >= limit)
                break;
        }
        Destroy(gameObject);
    }

	void Start () {
        coll = GetComponent<CircleCollider2D>();
    }

    public void StartBlow()
    {
        StartCoroutine("blow");
    }
}