using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Renderer _renderer;

    [SerializeField] private GameObject explosionRadious;

    [SerializeField] private float explosionTime;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        StartCoroutine(ExplodeBomb(explosionTime));
        StartCoroutine(SelfDestroy(explosionTime + 1f));
    }

    public void ThrowForce(Vector2 direction)
    {
        _rigidbody2D.velocity = direction;
    }

    private IEnumerator ExplodeBomb(float delay)
    {
        yield return new WaitForSeconds(delay);

        _renderer.enabled = false;

        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _rigidbody2D.isKinematic = true;

        explosionRadious.SetActive(true);
    }

    private IEnumerator SelfDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        this.GameObject().SetActive(false);
    }
}
