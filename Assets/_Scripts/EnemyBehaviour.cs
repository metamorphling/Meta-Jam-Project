using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    public float attackPeriodicity = 1f;
    public float attackStrengthX = 10f;
    public float attackStrengthY = 10f;
    public GameObject projectile;
    bool isWokenUp = false;
    bool isDying = false;
    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
    }

    IEnumerator attack()
    {
        while (isDying == false)
        {
            yield return new WaitForSeconds(attackPeriodicity);
            GameObject obj = Instantiate(projectile);

            obj.transform.position = transform.position;
            obj.GetComponent<Rigidbody2D>().velocity += new Vector2(attackStrengthX, attackStrengthY);
        }
    }

    IEnumerator explode()
    {
        anim.SetTrigger("monster_die");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void Die()
    {
        isDying = true;
        StartCoroutine("explode");
    }

    public void WakeUp()
    {
        isWokenUp = true;
        gameObject.layer = 0;
        anim.SetBool("monster_attack", true);
        StartCoroutine("attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Damage")
            StartCoroutine("explode");
    }
}
