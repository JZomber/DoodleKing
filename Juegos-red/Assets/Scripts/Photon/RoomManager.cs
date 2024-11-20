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

    private IEnumerator DisconnectPlayers()
    {
        yield return new WaitForSeconds(2f);

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
    }

    private void EndGame()
    {
        timerManager.OnGameFinished -= EndGame;
        gameplayCallBacks.OnMatchCanceled -= EndGame;

        StartCoroutine(DisconnectPlayers());
    }
}
