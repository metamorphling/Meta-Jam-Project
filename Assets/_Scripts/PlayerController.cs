using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float movementSpeed;
    public float moveModifier = 0;
    public float jumpForce = 2.0f;

    float inputAxis;
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool spriteIsRight = true;
    bool isGrounded = true;
    bool doJump = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        ProcessInput();
        ProcessSprite();
    }

    public void MoveLeft()
    {
        moveModifier = -0.8f;
    }

    public void MoveRight()
    {
        moveModifier = 0.8f;
    }

    public void MoveUp()
    {
        moveModifier = 0;
    }

    public void MoveDown()
    {
        moveModifier = 0;
    }

    public void Jump()
    {
        if (isGrounded == true)
        {
            doJump = true;
        }
    }

    public void Action()
    {
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        isGrounded = true;
    }

    void ProcessInput()
    {
        //inputAxis = Input.GetAxis("Horizontal");
        if (moveModifier == 0)
            inputAxis = Mathf.Lerp(inputAxis, 0, 0.01f);
        else
            inputAxis += Time.deltaTime * moveModifier;

        if (inputAxis >= 1)
            inputAxis = 1;
        else if (inputAxis <= -1)
            inputAxis = -1;
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

        if (doJump == true)
        {
            rb.velocity += Vector2.up * jumpForce;
            isGrounded = false;
            doJump = false;
        }
    }
}
