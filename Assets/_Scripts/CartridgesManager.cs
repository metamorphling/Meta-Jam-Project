using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgesManager : MonoBehaviour {

    public AudioClip insert, hover;
    AudioSource au;

    public void PlayInser()
    {
        au.clip = insert;
        au.Play();
    }

    public void PlayHover()
    {
        au.clip = hover;
        au.Play();
    }

    void Start () {
        au = GetComponent<AudioSource>();
    }
}
