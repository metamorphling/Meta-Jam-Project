using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour {

    Rigidbody2D rb;
    bool isRight;

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("SelfDestruct");
	}

    public void IsRight(bool direction)
    {
        isRight = direction;
    }

    void Update()
    {
        if (isRight == true)
        {
            if (rb.velocity.x <= 0)
                Destroy(gameObject);
        }
        else
        {
            if (rb.velocity.x >= 0)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyBehaviour scr = collision.transform.GetComponent<EnemyBehaviour>();
            if (scr)
                scr.Die();
        }
    }
}
