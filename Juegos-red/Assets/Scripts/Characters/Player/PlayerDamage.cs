using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerDamage : MonoBehaviour, IDamageable
{
    [Header("Player Movement")] 
    [SerializeField] private PlayerMovement playerMovement;
    
    [Header("Taking Damage")] 
    [SerializeField] private float damageCD;
    
    [Header("Animation")] 
    [SerializeField] private Animator animator;

    private Vector2 _playerSpawn;

    private bool _isHit;
    
    private BoxCollider2D _boxCollider2D;

    private Rigidbody2D _rigidbody2D;

    private GameObject _bombExplosionRef;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        _playerSpawn = gameObject.transform.position;
        
        animator.SetBool("isAlive", true);
        
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void PlayerSpawnRelocate()
    {
        _playerSpawn = gameObject.transform.position;
    }
    
    public void TakeDamage() // El player recibe daño
    {
        animator.SetTrigger("isHit"); 
        playerMovement.KnockBack(_bombExplosionRef);
        _bombExplosionRef = null;
        
        LoseControl();
    }
    
    private void LoseControl() // El player no puede moverse
    {
        playerMovement.canMove = false;
        animator.SetBool("isAlive", false);
        _boxCollider2D.enabled = false;
        
        StartCoroutine(Respawn(2f));
    }

    private IEnumerator Respawn(float delay) // Re-posición del player + animación
    {
        
        yield return new WaitForSeconds(delay);

        _rigidbody2D.velocity = new Vector2(0, 0);
        gameObject.transform.position = _playerSpawn;
        _boxCollider2D.enabled = true;
        
        animator.SetTrigger("isRespawn");
        animator.SetBool("isAlive", true);
        
        StartCoroutine(PlayerCanMove(1f));
    }
    
    private IEnumerator CoolDownHit(float delay) // CoolDown antes de que se pueda colisionar nuevamente
    {
        yield return new WaitForSeconds(delay);
        _isHit = false;
    }

    private IEnumerator PlayerCanMove(float delay) //Llamado en el spawn del player
    {
        yield return new WaitForSeconds(delay);
        playerMovement.canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion") && !_isHit)
        {
            _isHit = true;

            _bombExplosionRef = collision.gameObject;
            TakeDamage();

            StartCoroutine(CoolDownHit(1f));
        }
        else if (collision.CompareTag("MercuryExplosion"))
        {
            _isHit = true;

            PhotonView ownerPV = GetComponent<PhotonView>(); // Gets the component from the player that collided
            if (ownerPV != null)
            {
                int damagePoints = FindObjectOfType<PowerUpsManager>().GetMercuryBombDamage;
            
                ScoreManager.instance.RemoveScorePoints(ownerPV.Owner.ActorNumber, damagePoints);
            }

            _bombExplosionRef = collision.gameObject;
            TakeDamage();

            StartCoroutine(CoolDownHit(1f));
        }
    }
}
