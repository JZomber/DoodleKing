using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using Unity.VisualScripting;

public class TimerManager : MonoBehaviourPunCallbacks
{
    [Header("Configuracion de tiempo")]
    public float gameTime = 60f; // Duracion del juego en segundos
    private float remainingTime;

    [SerializeField] private int nextPowerUpTime = 10;

    [Header("UI Elements")]
    public TMP_Text timerText;

    private bool gameEnded = false;

    private GameplayCallBacks gameplayCallBacks;

    public event Action OnGameFinished;

    public event Action OnSpawnPowerUp;

    private void Start()
    {
        remainingTime = gameTime;

        timerText.text = "??";

        gameplayCallBacks = FindAnyObjectByType<GameplayCallBacks>();
        if (gameplayCallBacks != null)
        {
            gameplayCallBacks.OnMatchBeging += InitializeTimer;
            gameplayCallBacks.OnMatchCanceled += MatchCanceled;
        }
    }

    private IEnumerator GameTimer()
    {
        while (remainingTime > 0 && !gameEnded)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;

            nextPowerUpTime--;

            if (nextPowerUpTime <= 0)
            {
                OnSpawnPowerUp?.Invoke();
                nextPowerUpTime = 10;
            }

            photonView.RPC("UpdateTimer", RpcTarget.All, remainingTime);
        }
        if (!gameEnded)
        {
            photonView.RPC("EndGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void UpdateTimer(float time)
    {
        timerText.text = $"{time}";
    }
    
    private void MatchCanceled()
    {
        photonView.RPC("EndGame", RpcTarget.All);
    }

    [PunRPC]
    private void EndGame()
    {
        gameEnded = true;
        
        if (!gameplayCallBacks.matchCanceled)
        {
            OnGameFinished?.Invoke();
        }

        gameplayCallBacks.OnMatchCanceled -= EndGame;
    }

    private void InitializeTimer()
    {
        StartCoroutine(GameTimer());

        gameplayCallBacks.OnMatchBeging -= InitializeTimer;
    }

    public void ExtraTime(PowerUp powerUp, float time)
    {
        if (powerUp.powerUpName == "ExtraTime" && PhotonNetwork.IsMasterClient && !gameEnded)
        {
            photonView.RPC("AddExtraTime", RpcTarget.All, time);
        }
    }

    [PunRPC]
    private void AddExtraTime(float time)
    {
        remainingTime += time;
    }
}
