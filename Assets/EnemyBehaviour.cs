﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    public float attackPeriodicity = 1f;
    public float attackStrengthX = 10f;
    public float attackStrengthY = 10f;
    public GameObject projectile;
    bool isWokenUp = false;
    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
    }

    IEnumerator attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackPeriodicity);
            GameObject obj = Instantiate(projectile);
            obj.transform.position = transform.position;
            obj.GetComponent<Rigidbody2D>().velocity += new Vector2(attackStrengthX, attackStrengthY);
        }
    }

    public void WakeUp()
    {
        isWokenUp = true;
        anim.SetBool("monster_attack", true);
        StartCoroutine("attack");
    }
}