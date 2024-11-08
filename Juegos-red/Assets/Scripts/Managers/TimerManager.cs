using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TimerManager : MonoBehaviourPunCallbacks
{
    [Header("Configuración de tiempo")]
    public float gameTime = 60f; // Duración del juego en segundos
    private float remainingTime;

    [Header("UI Elements")]
    public TMP_Text timerText;
    public TMP_Text resultText; // Texto para mostrar el resultado

    private bool gameEnded = false;

    private void Start()
    {
        remainingTime = gameTime;

        // Solo el MasterClient será responsable de actualizar el temporizador
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameTimer());
        }
    }

    private IEnumerator GameTimer()
    {
        // Mientras haya tiempo restante y el juego no haya terminado
        while (remainingTime > 0 && !gameEnded)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;

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
        // Actualizar la visualización del temporizador en pantalla
        timerText.text = $"{time}";
    }

    [PunRPC]
    private void EndGame()
    {
        gameEnded = true;

        // Solo el MasterClient calcula el resultado
        if (PhotonNetwork.IsMasterClient)
        {
            Player winner = GetWinner();
            photonView.RPC("DisplayResult", RpcTarget.All, winner == PhotonNetwork.LocalPlayer);
        }
    }

    private Player GetWinner()
    {
        // Obtener la puntuación de los jugadores y compararlas

        int scorePlayer1 = ScoreManager.instance.ScorePlayer1;
        int scorePlayer2 = ScoreManager.instance.ScorePlayer2;

        if (scorePlayer1 > scorePlayer2)
        {
            return PhotonNetwork.PlayerList[0]; // Player 1 won.
        }
        else if (scorePlayer2 > scorePlayer1)
        {
            return PhotonNetwork.PlayerList[1]; // Player 2 won.
        }

        return null;
    }

    [PunRPC]
    private void DisplayResult(bool isWinner)
    {
        // Mostrar el mensaje de victoria o derrota según el resultado
        resultText.text = isWinner ? "¡Victoria!" : "Derrota";
        resultText.gameObject.SetActive(true);
    }
}
