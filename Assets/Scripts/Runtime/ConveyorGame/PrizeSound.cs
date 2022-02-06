using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeSound : MonoBehaviour
{
    [SerializeField] private AudioSource sound;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        sound.Play();
    }
}
