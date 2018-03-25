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


    /* character sprites */
    Sprite zelda_shield;

    enum Character
    {
        None,
        Mario,
        Zelda,
        Sonic,
        Kirby
    }

    Character currentCharacter;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
		if (AbleToMove) {
			
			ProcessInput ();
			ProcessSprite ();
		}
	}

    public void MoveLeft()
    {
        moveModifier = -0.2f;
    }

    public void MoveRight()
    {
        moveModifier = 0.2f;
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

    void Shield()
    {

    }

    void Sword()
    {

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
            Shield();
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
            Jump();
        }
        else if (currentCharacter == Character.Zelda)
        {
            Sword();
        }
        else if(currentCharacter == Character.Sonic)
        {
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
        spriteIsRight = rb.velocity.x >= 0 ? true : false;
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
            /* adding appropriate event trigger */
            //EventTrigger evTrigger = GetComponent<EventTrigger>();
            //EventTrigger.Entry entry = evTrigger.triggers[0];
            //entry.eventID = EventTriggerType.PointerDown;
            //GameObject toSet = itemPrefabs[buttonOffset];
            //entry.callback.AddListener((data) => { gameEditor.ChangeCurrentTile((GameObject)toSet); });
            //go.GetComponent<Image>().sprite = itemPrefabs[buttonOffset].GetComponent<SpriteRenderer>().sprite;
        }
        else if (currentCharacter == Character.Zelda)
        {

        }
        else if (currentCharacter == Character.Sonic)
        {

        }
        else if (currentCharacter == Character.Kirby)
        {

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


	void OnCollisionEnter2D( Collision2D  c)
	{
		if (c.gameObject.name == "key") {
			HasKey = true;

			Destroy (c.gameObject);

		}

		if (c.gameObject.name == "door" && HasKey == true) {

			UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
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
