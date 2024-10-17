using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView _playerView;
    
    [Header("Movement Values")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private float _moveInput;
    [SerializeField] private int extraJumpValue;
    private int _extraJumpCount;
    
    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadious;
    [SerializeField] private LayerMask whatIsGround; 
    private bool _isGrounded;

    [Header("Taking Damage")]
    public bool canMove;
    [SerializeField] private Vector2 knockbackDir;
    [SerializeField] private int  bounceForce;
    
    
    [Header("Animation")] 
    [SerializeField] private Animator animator;
    
    private Rigidbody2D _rigidbody2D;
    
    private bool _facingRight = true;
    public bool FacingRight => _facingRight;
    
    public UnityEvent onLandEvent;

    private void Awake()
    {
        _playerView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _extraJumpCount = extraJumpValue;
        
        if (onLandEvent == null)
        {
            onLandEvent = new UnityEvent();
        }
    }

    private void Update()
    {
        if (_playerView.IsMine) // Determina si el punto de vista es mio y no de la otra instancia de jugador.
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && canMove) // Input de salto
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isGrounded", false);

                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount > 0 && !_isGrounded && canMove) // Input doble salto
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isGrounded", false);
                
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            
                _extraJumpCount--;
            }
        
            if (!canMove)
            {
                _moveInput = 0;
                animator.SetFloat("Speed", _moveInput);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_playerView.IsMine)
        {
            if (canMove)
            {
                _moveInput = Input.GetAxis("Horizontal");

                _rigidbody2D.velocity = new Vector2(_moveInput * speed, _rigidbody2D.velocity.y);
            
                animator.SetFloat("Speed", Mathf.Abs(_moveInput));
            }

            if (!_facingRight && _moveInput > 0)
            {
                Flip();
            }
            else if (_facingRight && _moveInput < 0)
            {
                Flip();
            }
        
            bool wasGrounded = _isGrounded;
            _isGrounded = false;
        
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadious, whatIsGround);
        
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    _isGrounded = true;
                    if (!wasGrounded)
                    {
                        onLandEvent.Invoke();
                        _extraJumpCount = extraJumpValue; // Reinicia el número de saltos extras al tocar un layer que cuente como "Piso"
                    }
                }
            }
        }
    }

    public void KnockBack(GameObject gameObject) // KnockBack cuando el player recibe un ataque enemigo
    {
        if (_playerView.IsMine)
        {
            Vector2 knockbackDirection;
        
            if (_facingRight)
            {
                knockbackDirection = new Vector2(-knockbackDir.x * gameObject.transform.position.x, knockbackDir.y);
            }
            else 
            {
                knockbackDirection = new Vector2(knockbackDir.x * gameObject.transform.position.x, knockbackDir.y);
            }

            _rigidbody2D.velocity = knockbackDirection;
        }
    }

    public void OnLanding() // Cada vez que toca el suelo o una plataforma
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isGrounded", true);
    }

    private void Flip() // Invierte el sprite del player según la dirección
    {
        if (_playerView.IsMine)
        {
            _facingRight = !_facingRight;
        
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public bool CheckIsFacingRight()
    {
        if (_playerView.IsMine)
        {
            bool result;

            if (_facingRight == true)
            {
                result = true;
                return result;
            }
        }
        
        return default;   
    }
}
