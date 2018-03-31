using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float movementSpeed;
    public float moveModifier = 0;
    public float jumpForce = 2.0f;
	public int FallModifier = 3;
	public int LowFallModifier = 4;
	bool JumpHeld = false;
	public Transform groundCheckPoint;
	public LayerMask groundLayer;
    public GameObject marioFire;

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
    public float sonicImpulse = 30f, kirbyImpulse = 13f;

    /* character sprites */
    Sprite zelda_shield;

    /* abilities/inventory */
    BoxCollider2D shieldCollider, attackCollider;
    int last_status;
    bool isRolling = false;
    bool isShielded = false;
    Collider2D sonicCollider, blowCollider;
    float shieldOffset, attackOffset, blowOffset;
    bool isKilled = false;
    GameObject pipeEnter = null;
    bool marioGoPipe = false;
    bool marioDown = true;
    float marioGoTime = 0, marioMaxTime = 1f;
    Vector2 startPos, endPos;

    /* sounds */
    public AudioClip keyFound, Jump1, Jump2, zeldaSword1, zeldaSword2, zeldaShield, kirbyFloat, kirbyShoot, gameOver, pipe;
    AudioSource au;

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
        blowCollider = transform.GetChild(3).GetComponent<BoxCollider2D>();
        shieldOffset = shieldCollider.offset.x;
        attackOffset = attackCollider.offset.x;
        blowOffset = blowCollider.offset.x;
        sonicCollider = GetComponent<CircleCollider2D>();
        au = GetComponent<AudioSource>();
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
            if (pipeEnter != null)
            {
                startPos = transform.position;
                endPos = transform.position - transform.up * Mathf.Abs(transform.position.y - Mathf.Abs(transform.localScale.y * GetComponent<SpriteRenderer>().sprite.bounds.size.y));
                marioGoPipe = true;
                marioDown = true;
                GetComponent<SpriteRenderer>().sortingOrder = -60;
                GetComponent<CapsuleCollider2D>().enabled = false;
                au.clip = pipe;
                au.Play();
            }
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
                int rand = Random.Range(0,2);
                if (rand == 0)
                    au.clip = Jump1;
                else
                    au.clip = Jump2;
                au.Play();

                GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpForce);

				doJump = true;

				count++;

				JumpHeld = true;
			}
		}
    }

    void Fire()
    {
        GameObject obj = Instantiate(marioFire);
        if (spriteIsRight)
        {
            obj.transform.position = transform.position + Vector3.right * 1f;
            obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
            obj.GetComponent<FireBehaviour>().IsRight(true);
        }
        else
        {
            obj.transform.position = transform.position - Vector3.right * 1f;
            obj.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 10f, ForceMode2D.Impulse);
            obj.GetComponent<FireBehaviour>().IsRight(false);
        }
    }

    void Shield(bool press)
    {
        if (press == true)
        {
            isShielded = true;
            if (spriteIsRight == false)
                shieldCollider.offset = new Vector2(shieldOffset - 1.3f,0);
            else
                shieldCollider.offset = new Vector2(shieldOffset, 0);
            shieldCollider.enabled = true;
            last_status = anim.GetInteger("zelda");
            if (moveModifier != 0)
                anim.SetInteger("zelda", (int)ZeldaAnimation.Block);
            else
                anim.SetInteger("zelda", (int)ZeldaAnimation.BlockStand);
        }
        else
        {
            isShielded = false;
            if (spriteIsRight == false)
                shieldCollider.offset = new Vector2(shieldOffset - 1.3f, 0);
            else
                shieldCollider.offset = new Vector2(shieldOffset, 0);
            anim.SetInteger("zelda", last_status);
            shieldCollider.enabled = false;
        }
    }

    IEnumerator Sword()
    {
        if (spriteIsRight == false)
            attackCollider.offset = new Vector2(attackOffset - 2, 0);
        else
            attackCollider.offset = new Vector2(attackOffset, 0);
        attackCollider.enabled = true;
        int rand = Random.Range(0,2);
        if (rand == 0)
            au.clip = zeldaSword1;
        else
            au.clip = zeldaSword2;
        au.Play();
        yield return new WaitForSeconds(0.3f);
        attackCollider.enabled = false;
    }

    IEnumerator Roll()
    {
        anim.SetInteger("sonic", (int)SonicAnimation.Roll);
        yield return new WaitForSeconds(0.5f);
        isRolling = true;
        sonicCollider.enabled = true;
        rb.AddForce(new Vector2(sonicImpulse, 0), ForceMode2D.Impulse);
    }

    void Inhale()
    {
        rb.gravityScale = 0.3f;
        au.clip = kirbyFloat;
        au.Play();
        rb.AddForce(new Vector2(0, kirbyImpulse), ForceMode2D.Impulse);
    }

    IEnumerator Exhale()
    {
        moveModifier = 0;
        if (spriteIsRight == false)
            blowCollider.offset = new Vector2(blowOffset - 2.6f, 0);
        else
            blowCollider.offset = new Vector2(blowOffset, 0);
        blowCollider.enabled = true;
        anim.SetTrigger("kirby_exhale");
        au.clip = kirbyShoot;
        au.Play();
        yield return new WaitForSeconds(1f);
        blowCollider.enabled = false;
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
            StartCoroutine("Roll");
        }
        else if(currentCharacter == Character.Kirby)
        {
            StartCoroutine("Exhale");
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
            anim.SetTrigger("kirby_inhale");
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
        if (isKilled == true)
        {
            transform.Rotate(Vector3.forward * 20);
            return;
        }
        if (isRolling == true)
        {
            if (rb.velocity == Vector2.zero)
            {
                isRolling = false;
                anim.SetInteger("sonic", (int)SonicAnimation.Idle);
                sonicCollider.enabled = false;
            }
            return;
        }

        if (currentCharacter == Character.Kirby && rb.gravityScale !=1)
        {
            rb.gravityScale = 1;
        }

        if (marioGoPipe == true)
        {
            float perc = marioGoTime / marioMaxTime;
            //perc = Mathf.Sin(Mathf.PI * perc * 0.5f);
            marioGoTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, perc);
            if (perc >= 1)
            {
                marioGoTime = 0;
                if (marioDown == true)
                {
                    transform.position = pipeEnter.transform.position;// - pipeEnter.transform.up * (Mathf.Abs(pipeEnter.transform.localScale.y * pipeEnter.GetComponent<SpriteRenderer>().sprite.bounds.size.y) / 2);
                    startPos = transform.position;
                    endPos = transform.position + transform.up * Mathf.Abs((transform.position.y + Mathf.Abs(transform.localScale.y * GetComponent<SpriteRenderer>().sprite.bounds.size.y)) * 0.8f);
                    au.clip = pipe;
                    au.Play();
                    marioDown = false;
                }
                else
                {
                    GetComponent<CapsuleCollider2D>().enabled = true;
                    marioGoPipe = false;
                    GetComponent<SpriteRenderer>().sortingOrder = 1;
                    marioDown = true;
                }
            }
        }

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
        if (c.gameObject.tag == "Enemy")
        {
            Killed();
        }

        if (c.gameObject.tag == "Pipe")
        {
            Debug.Log("coll pipe");
            foreach (GameObject pipe in GameObject.FindGameObjectsWithTag("Pipe"))
            {
                if (pipe != c.gameObject)
                {
                    pipeEnter = pipe;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.name == "Pipe")
        {
            pipeEnter = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "key")
        {
            GetComponent<AudioSource>().clip = keyFound;
            GetComponent<AudioSource>().Play();
            HasKey = true;
            Destroy(c.gameObject);
            DoorBehaviour door = GameObject.Find("door").GetComponent<DoorBehaviour>();
            door.OpenDoor();

            GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in array)
            {
                obj.GetComponent<EnemyBehaviour>().WakeUp();
            }
        }

        if (c.gameObject.name == "door" && HasKey == true)
        {
            StartCoroutine("NextLevel");
        }

        if (c.gameObject.tag == "Enemy" && isShielded == true)
        {
            au.clip = zeldaShield;
            au.Play();
            Destroy(c.gameObject);
        }

        if (c.gameObject.tag == "Wall" && isRolling == true)
        {
            c.gameObject.GetComponent<BrickBehaviour>().Break();
        }

        if (c.gameObject.tag == "Flower")
        {
            GetComponent<Animator>().SetBool("mario_fire", true);
            Destroy(c.gameObject);
        }
    }

    IEnumerator RestartLevel()
    {
        GameObject gl = GameObject.Find("Glitch");
        yield return new WaitForSeconds(2f);
        gl.GetComponent<Animator>().SetTrigger("playGlitch");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator NextLevel()
    {
        GameObject gl = GameObject.Find("Glitch");
        gl.GetComponent<Animator>().SetTrigger("playGlitch");
        yield return new WaitForSeconds(0.3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Killed()
    {
        isKilled = true;
        au.clip = gameOver;
        au.Play();
        GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine("RestartLevel");
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
