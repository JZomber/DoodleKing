using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;
using Unity.VisualScripting;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private Camera mainCamera;

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
        Canvas playerCanvas = playerPrefab[index].GetComponentInChildren<Canvas>();
        playerCanvas.worldCamera = mainCamera;
    }
}