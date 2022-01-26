using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(DestroyItem(col.gameObject));
    }

    private IEnumerator DestroyItem(GameObject obj)
    {
        //TODO: Play animation
        //Wait for animation
        
        Destroy(obj);
        yield return null;
    }
}
