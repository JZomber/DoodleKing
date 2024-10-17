using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private Transform[] playerSpawns;

    private int index;
    
    private void Start()
    {
        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 1:
                index = 0; 
                break;
            case 2:
                index = 1;
                break;
        }
        
        PhotonNetwork.Instantiate(playerPrefab[index].name, playerSpawns[index].transform.position, quaternion.identity);
    }
}