using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public float movementSpeed;
    public float moveModifier = 0;
    public float jumpForce = 2.0f;
	public int FallModifier = 3;
	public int LowFallModifier = 4;
	bool JumpHeld = false;
	public Transform groundCheckPoint;
	public LayerMask groundLayer;
    float inputAxis;
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool spriteIsRight = true;
    bool isGrounded = true;
    bool doJump = false;
	bool HasKey = false;
	int facing = 1;
	int count = 0;
	bool isTouchingGround = true;
	bool AbleToMove = false;
    Animator anim;


    /* character sprites */
    Sprite zelda_shield;

    /* abilities/inventory */
    BoxCollider2D shieldCollider, attackCollider;
    int last_status;

    enum Character
    {
        None,
        Mario,
        Zelda,
        Sonic,
        Kirby
    }

    enum MarioAnimation
    {
        Idle = 1,
        Walk
    }
    enum ZeldaAnimation
    {
        Idle = 1,
        Attack,
        Block,
        Walk,
        BlockStand
    }
    enum SonicAnimation
    {
        Idle = 1,
        Walk,
        Roll
    }
    enum KirbyAnimation
    {
        Idle = 1,
        Walk,
        Blow,
        Suck
    }

    Character currentCharacter;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shieldCollider = transform.GetChild(1).GetComponent<BoxCollider2D>();
        attackCollider = transform.GetChild(2).GetComponent<BoxCollider2D>();
    }

    void Update() {
		if (AbleToMove) {
			
			ProcessInput ();
			ProcessSprite ();
		}
	}

    public void MoveLeft()
    {
        spriteIsRight = false;
        moveModifier = -0.2f;
        if (currentCharacter == Character.Mario)
        {
            anim.SetInteger("mario", (int)MarioAnimation.Walk);
        }
        else if (currentCharacter == Character.Zelda)
        {
            anim.SetInteger("zelda", (int)ZeldaAnimation.Walk);
        }
        else if (currentCharacter == Character.Sonic)
        {
            anim.SetInteger("sonic", (int)SonicAnimation.Walk);
        }
        else if (currentCharacter == Character.Kirby)
        {
            anim.SetInteger("kirby", (int)KirbyAnimation.Walk);
        }
    }

    public void MoveRight()
    {
        spriteIsRight = true;
        moveModifier = 0.2f;
        if (currentCharacter == Character.Mario)
        {
            anim.SetInteger("mario", (int)MarioAnimation.Walk);
        }
        else if (currentCharacter == Character.Zelda)
        {
            anim.SetInteger("zelda", (int)ZeldaAnimation.Walk);
        }
        else if (currentCharacter == Character.Sonic)
        {
            anim.SetInteger("sonic", (int)SonicAnimation.Walk);
        }
        else if (currentCharacter == Character.Kirby)
        {
            anim.SetInteger("kirby", (int)KirbyAnimation.Walk);
        }
    }

    public void MoveUp()
    {
        moveModifier = 0;
        if (currentCharacter == Character.Mario)
        {
            anim.SetInteger("mario", (int)MarioAnimation.Idle);
        }
        else if (currentCharacter == Character.Zelda)
        {
            anim.SetInteger("zelda", (int)ZeldaAnimation.Idle);
        }
        else if (currentCharacter == Character.Sonic)
        {
            anim.SetInteger("sonic", (int)SonicAnimation.Idle);
        }
        else if (currentCharacter == Character.Kirby)
        {
            anim.SetInteger("kirby", (int)KirbyAnimation.Idle);
        }
    }

    public void MoveDown()
    {
        moveModifier = 0;
        if (currentCharacter == Character.Mario)
        {
            anim.SetInteger("mario", (int)MarioAnimation.Idle);
        }
        else if (currentCharacter == Character.Zelda)
        {
            anim.SetInteger("zelda", (int)ZeldaAnimation.Idle);
        }
        else if (currentCharacter == Character.Sonic)
        {
            anim.SetInteger("sonic", (int)SonicAnimation.Idle);
        }
        else if (currentCharacter == Character.Kirby)
        {
            anim.SetInteger("kirby", (int)KirbyAnimation.Idle);
        }
    }

    public void Jump()
    {
		if (count < 1 && !doJump) {
			{
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpForce);

				doJump = true;

				count++;

				JumpHeld = true;
			}
		}
    }

    void Fire()
    {

    }

    void Shield(bool press)
    {
        if (press == true)
        {
            if (spriteIsRight == false)
                shieldCollider.offset -= new Vector2(1.3f,0);
            shieldCollider.enabled = true;
            last_status = anim.GetInteger("zelda");
            if (moveModifier != 0)
                anim.SetInteger("zelda", (int)ZeldaAnimation.Block);
            else
                anim.SetInteger("zelda", (int)ZeldaAnimation.BlockStand);
        }
        else
        {
            if (spriteIsRight == false)
                shieldCollider.offset += new Vector2(1.3f, 0);
            anim.SetInteger("zelda", last_status);
            shieldCollider.enabled = false;
        }
    }

    IEnumerator Sword()
    {
        if (spriteIsRight == false)
            attackCollider.offset -= new Vector2(2, 0);
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        attackCollider.enabled = false;
        if (spriteIsRight == false)
            attackCollider.offset += new Vector2(2, 0);
    }

    void Roll()
    {

    }

    void Inhale()
    {

    }

    void Exhale()
    {

    }

    public void ActionDownUnpressed()
    {
        if (currentCharacter == Character.Mario)
        {
        }
        else if (currentCharacter == Character.Zelda)
        {
            Shield(false);
        }
        else if (currentCharacter == Character.Sonic)
        {
        }
        else if (currentCharacter == Character.Kirby)
        {
        }
    }

    public void ActionDownPressed()
    {
        if (currentCharacter == Character.Mario)
        {
            Fire();
        }
        else if (currentCharacter == Character.Zelda)
        {
            Shield(true);
        }
        else if(currentCharacter == Character.Sonic)
        {
            Roll();
        }
        else if(currentCharacter == Character.Kirby)
        {
            Exhale();
        }
    }

    public void ActionUpUnpressed()
    {
        Debug.Log("curr " + currentCharacter);
        if (currentCharacter == Character.Mario)
        {
            JumpLetgo();
        }
        else if (currentCharacter == Character.Zelda)
        {

        }
        else if (currentCharacter == Character.Sonic)
        {
            JumpLetgo();
        }
        else if (currentCharacter == Character.Kirby)
        {
        }
    }

    public void ActionUpPressed()
    {
        Debug.Log("curr " + currentCharacter);
        if (currentCharacter == Character.Mario)
        {
            anim.SetTrigger("mario_jump");
            Jump();
        }
        else if (currentCharacter == Character.Zelda)
        {
            anim.SetTrigger("zelda_attack");
            StartCoroutine("Sword");
        }
        else if(currentCharacter == Character.Sonic)
        {
            anim.SetTrigger("sonic_jump");
            Jump();
        }
        else if(currentCharacter == Character.Kirby)
        {
            Inhale();
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        isGrounded = true;
    }

    void ProcessInput()
    {
        //inputAxis = Input.GetAxis("Horizontal");
     /*   if (moveModifier == 0)
            inputAxis = Mathf.Lerp(inputAxis, 0, 0.01f);
        else
            inputAxis += Time.deltaTime * moveModifier;

        if (inputAxis >= 1)
            inputAxis = 1;
        else if (inputAxis <= -1)
            inputAxis = -1;

	
	 

		if (Input.GetButtonDown("Jump") && count < 1 && !doJump)
		{
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce) ;

				doJump = true;
			
				count++;
			}
		}*/
    }

    void ProcessSprite()
    {
        if (spriteIsRight == false)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    public void ChangeLevel(int type)
    {
        currentCharacter = (Character)type;

        if (currentCharacter == Character.Mario)
        {
            anim.SetInteger("zelda", 0);
            anim.SetInteger("sonic", 0);
            anim.SetInteger("kirby", 0);
            anim.SetInteger("mario", (int)MarioAnimation.Idle);

        }
        else if (currentCharacter == Character.Zelda)
        {
            anim.SetInteger("mario", 0);
            anim.SetInteger("sonic", 0);
            anim.SetInteger("kirby", 0);
            anim.SetInteger("zelda", (int)ZeldaAnimation.Idle);
        }
        else if (currentCharacter == Character.Sonic)
        {
            anim.SetInteger("mario", 0);
            anim.SetInteger("zelda", 0);
            anim.SetInteger("kirby", 0);
            anim.SetInteger("sonic", (int)SonicAnimation.Idle);
        }
        else if (currentCharacter == Character.Kirby)
        {
            anim.SetInteger("mario", 0);
            anim.SetInteger("zelda", 0);
            anim.SetInteger("sonic", 0);
            anim.SetInteger("kirby", (int)KirbyAnimation.Idle);
        }
    }

    private void FixedUpdate()
    {
		if (AbleToMove) {
			float move = movementSpeed * moveModifier;

			if (rb.velocity.y < 0) {
				rb.velocity += Vector2.up * Physics2D.gravity.y * (FallModifier - 1) * Time.deltaTime;
			} else if (rb.velocity.y > 0 && !JumpHeld) {
				rb.velocity += Vector2.up * Physics2D.gravity.y * (LowFallModifier - 1) * Time.deltaTime;
			}

			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (1, 0) * move, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, -5, 5), GetComponent<Rigidbody2D>().velocity.y);
            isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, 1f, groundLayer);
            if (isTouchingGround == true && doJump == true && rb.velocity.y == 0)
            {
                count = 0;
                doJump = false;
            }

        }

    }

    void OnCollisionEnter2D(Collision2D c)
    {
	    if (c.gameObject.name == "key")
        {
		    HasKey = true;
		    Destroy (c.gameObject);

            GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in array)
            {
                obj.GetComponent<EnemyBehaviour>().WakeUp();
            }
	    }

        if (c.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "door" && HasKey == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (c.gameObject.tag == "Enemy")
        {
            Destroy(c.gameObject);
        }
    }

    public void AllowMovement()
	{
		AbleToMove = true;
	}

	public void JumpLetgo()
	{
		JumpHeld = false;
	}
}
