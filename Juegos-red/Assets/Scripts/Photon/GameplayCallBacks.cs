using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Unity.VisualScripting;

public class GameplayCallBacks : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject CallBackCanvas;
    [SerializeField] private TMPro.TMP_Text errorText;
    [SerializeField] private CallBackMessages callBackMessages; // S.Object with different messages

    private TimerManager timerManager;

    private bool matchEnded = false;

    private void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.OnGameFinished += GameFinished;
        }
    }

    public event Action OnMatchBeging;
    public event Action OnMatchCanceled;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnMatchBeging?.Invoke();
        Debug.Log("Nuevo jugador ha entrado a la partida");
        //base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!matchEnded)
        {
            OnMatchCanceled?.Invoke();
            ActivatePlayerLeftPopup();
            Debug.Log("Un jugador ha salido de la partida");
        }
        
        //base.OnPlayerLeftRoom(otherPlayer);
    }

    private void ActivatePlayerLeftPopup()
    {
        CallBackCanvas.SetActive(true);
        errorText.text = callBackMessages.OnPlayerDisconnected;
    }

    private void GameFinished()
    {
        timerManager.OnGameFinished -= GameFinished;
        matchEnded = true;
    }
}
