using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    
    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, quaternion.identity);
    }
}