using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehaviour : MonoBehaviour {

    public Sprite pressed, unpressed;
    SpriteRenderer selfSprite;

    private void Start()
    {
        selfSprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            selfSprite.sprite = pressed;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            selfSprite.sprite = unpressed;
        }
    }
}
