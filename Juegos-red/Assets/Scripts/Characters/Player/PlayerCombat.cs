using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Photon.Pun;
using Unity.Mathematics;

public class PlayerCombat : MonoBehaviour
{
    private PhotonView _playerView;
    
    [Header("Combat Values")] 
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _mercuryBombPrefab;
    [SerializeField] private Transform bombOrigin;
    [SerializeField] private float _bombCdTime;
    [SerializeField] private bool _isBombInCD;
    private bool isFacingRight;

    [SerializeField] private Vector2 _throwForce;

    private PlayerMovement _playerMovement;

    [SerializeField] private bool hasMercuryBomb = false;

    private void Awake()
    {
        _playerView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (_playerMovement == null)
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        if (_playerView.IsMine)
        {
            if (Input.GetKey(KeyCode.F) && !_isBombInCD)
            {
                Vector2 throwDirection;

                if (_playerMovement.CheckIsFacingRight())
                {
                    throwDirection = new Vector2(_throwForce.x, _throwForce.y);
                }
                else
                {
                    throwDirection = new Vector2(-_throwForce.x, _throwForce.y);
                }

                _isBombInCD = true;
                StartCoroutine(BombCoolDown(_bombCdTime));
                
                if (!hasMercuryBomb)
                {
                    var instance = PhotonNetwork.Instantiate(_bombPrefab.name, bombOrigin.position, quaternion.identity);
                    instance.GetComponent<BombController>().ThrowForce(throwDirection);
                }
                else 
                {
                    var instance = PhotonNetwork.Instantiate(_mercuryBombPrefab.name, bombOrigin.position, quaternion.identity);
                    instance.GetComponent<BombController>().ThrowForce(throwDirection);

                    hasMercuryBomb = false;
                }
            }
        }
    }

    private IEnumerator BombCoolDown(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isBombInCD = false;
    }

    public void MercuryBombPowerUp(PowerUp powerUp)
    {
        if (powerUp.powerUpName == "MercuryBomb")
        {
            hasMercuryBomb = true;
        }
    }
}
