using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowBehaviour : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Rigidbody2D obj = collision.GetComponent<Rigidbody2D>();
            if (obj.velocity.x > 0 || obj.velocity.x < 0)
            {
                obj.GetComponent<SpriteRenderer>().flipX = true;
                obj.velocity = -obj.velocity;
                obj.tag = "Damage";
            }
        }
    }
}
