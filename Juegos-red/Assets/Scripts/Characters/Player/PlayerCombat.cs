using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Command;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Values")] 
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private bool _isBombInCD;
    private Rigidbody2D _bombRB;
    private bool isFacingRight;

    [SerializeField] private Vector2 _throwForce = new Vector2(2, 2);

    private PlayerMovement _playerMovement;
    
    void Start()
    {
        _bombRB = _bombPrefab.GetComponent<Rigidbody2D>();

        if (_playerMovement == null)
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && !_isBombInCD)
        {
            Vector2 throwDirection;
            
            Instantiate(_bombPrefab);


            if (_playerMovement.CheckIsFacingRight())
            {
                throwDirection = new Vector2(_throwForce.x, _throwForce.y);
            }
            else
            {
                throwDirection = new Vector2(-_throwForce.x, _throwForce.y);
            }
            
            var throwCommand = new PhysicsBombThrowCommand(throwDirection, _bombRB);
            EventQueue.Instance.QueueCommands(throwCommand);
        }
    }
}
