using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private TimerManager timerManager;
    private GameplayCallBacks gameplayCallBacks;

    private void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.OnGameFinished += EndGame;
        }

        gameplayCallBacks = FindObjectOfType<GameplayCallBacks>();
        if (gameplayCallBacks != null)
        {
            gameplayCallBacks.OnMatchCanceled += EndGame;
        }
    }

    private void EndGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.LeaveRoom();
        }

        timerManager.OnGameFinished -= EndGame;
        gameplayCallBacks.OnMatchCanceled -= EndGame;
    }
}
