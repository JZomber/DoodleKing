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
    public TMP_Text resultText; // Texto para mostrar el resultado

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
            //Debug.Log("Timer subscripto al evento 'OnMatchBeging'");
        }

        //// Solo el MasterClient es responsable de actualizar el temporizador
        //if (PhotonNetwork.IsMasterClient && !gameStarted)
        //{
        //    StartCoroutine(GameTimer());
        //}
    }

    private IEnumerator GameTimer()
    {
        // Mientras haya tiempo restante y el juego no haya terminado
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

            // Actualizamos el tiempo para todos los jugadores
            photonView.RPC("UpdateTimer", RpcTarget.All, remainingTime);
        }

        // Fin del tiempo
        if (!gameEnded)
        {
            photonView.RPC("EndGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void UpdateTimer(float time)
    {
        // Actualizar la visualizacion del temporizador en pantalla
        timerText.text = $"{time}";
    }

    [PunRPC]
    private void EndGame()
    {
        gameEnded = true;

        gameplayCallBacks.OnMatchBeging -= InitializeTimer;

        // Solo el MasterClient calcula el resultado
        if (PhotonNetwork.IsMasterClient)
        {
            int winnerActorNumber = GetWinnerActorNumber();
            photonView.RPC("DisplayResult", RpcTarget.All, winnerActorNumber);
        }

        OnGameFinished?.Invoke();
    }

    private int GetWinnerActorNumber()
    {
        // Obtener la puntuacion de los jugadores y compararlas

        int scorePlayer1 = ScoreManager.instance.GetScorePlayer1;
        int scorePlayer2 = ScoreManager.instance.GetScorePlayer2;

        if (scorePlayer1 > scorePlayer2)
        {
            return PhotonNetwork.PlayerList[0].ActorNumber; // Player 1 won.
        }
        else if (scorePlayer2 > scorePlayer1)
        {
            return PhotonNetwork.PlayerList[1].ActorNumber; // Player 2 won.
        }

        return -1; // Tie case
    }

    [PunRPC]
    private void DisplayResult(int winnerActorNumber)
    {
        if (winnerActorNumber == -1)
        {
            resultText.text = "Empate!";
        }
        else
        {
            bool isWinner = PhotonNetwork.LocalPlayer.ActorNumber == winnerActorNumber; 
            resultText.text = isWinner ? "Victoria!" : "Derrota"; // Mostrar el mensaje de victoria o derrota seg�n el resultado
        }

        resultText.gameObject.SetActive(true);
    }

    private void InitializeTimer()
    {
        StartCoroutine(GameTimer());
    }

    public void ExtraTime(PowerUp powerUp, float time)
    {
        if (powerUp.powerUpName == "ExtraTime" && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("AddExtraTime", RpcTarget.All, time);
        }
    }

    [PunRPC]
    private void AddExtraTime(float time)
    {
        remainingTime += time;
        Debug.Log("AGREGADO EXTRA TIME");
    }
}
