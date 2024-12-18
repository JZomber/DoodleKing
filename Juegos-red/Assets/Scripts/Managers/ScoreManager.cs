using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ScoreManager : MonoBehaviourPun
{
    public static ScoreManager instance;

    [SerializeField] private TMP_Text textScoreplayer1;
    [SerializeField] private TMP_Text textScoreplayer2;

    private int scorePlayer1;
    private int scorePlayer2;

    public int GetScorePlayer1 => scorePlayer1;
    public int GetScorePlayer2 => scorePlayer2;

    private void Awake()
    {
        ScoreManager.instance = this;
    }

    public void AddScorePoints(int actorNumber, int amount)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            photonView.RPC("UpdatePlayerScores", RpcTarget.All, actorNumber, amount, ScoreOperation.AddPoints);
        }
    }

    public void RemoveScorePoints(int actorNumber, int amount)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            photonView.RPC("UpdatePlayerScores", RpcTarget.All, actorNumber, amount, ScoreOperation.SubtractPoints);
        }
    }

    [PunRPC]
    private void UpdatePlayerScores(int actorNumber, int amount, ScoreOperation operation)
    {
        switch (operation)
        {
            case ScoreOperation.AddPoints:

                if (actorNumber == 1)
                {
                    scorePlayer1 += amount;
                    textScoreplayer1.text = $"{scorePlayer1}";
                }
                else if (actorNumber == 2)
                { 
                    scorePlayer2 += amount;
                    textScoreplayer2.text = $"{scorePlayer2}";
                }

                break;

            case ScoreOperation.SubtractPoints:

                if (actorNumber == 1)
                {
                    scorePlayer1 -= amount;
                    textScoreplayer1.text = $"{scorePlayer1}";
                }
                else if (actorNumber == 2)
                { 
                    scorePlayer2 -= amount;
                    textScoreplayer2.text = $"{scorePlayer2}";
                }
            
                break;

            default:

                Debug.LogWarning("No se ejecutó ninguna operacion");

            break;
        }
    }

    public int GetWinnerActorNumber()
    {
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

    public enum ScoreOperation
    {
        AddPoints,
        SubtractPoints,
    }
}
