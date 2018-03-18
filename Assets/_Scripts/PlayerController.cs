using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float movementSpeed;

    float inputAxis;
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool spriteIsRight = true;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        ProcessInput();
        ProcessSprite();
    }

    void ProcessInput()
    {
        inputAxis = Input.GetAxis("Horizontal");
    }

    void ProcessSprite()
    {
        spriteIsRight = inputAxis >= 0 ? true : false;
        if (spriteIsRight == false)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * inputAxis * movementSpeed;
    }
}
