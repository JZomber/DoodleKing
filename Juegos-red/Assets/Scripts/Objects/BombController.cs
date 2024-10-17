using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(SelfDestroy(3f));
    }

    public void ThrowForce(Vector2 direction)
    {
        _rigidbody2D.velocity = direction;
    }

    private IEnumerator SelfDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        this.GameObject().SetActive(false);
    }
}
