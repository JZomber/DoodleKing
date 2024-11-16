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

    public void AddScorePoints(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            photonView.RPC("UpdatePlayerScores", RpcTarget.All, actorNumber, 0);
        }
    }

    public void RemoveScorePoints(int actorNumber, int amount)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            photonView.RPC("UpdatePlayerScores", RpcTarget.All, actorNumber, amount);
        }
    }

    [PunRPC]
    private void UpdatePlayerScores(int actorNumber, int amount)
    {
        //Debug.Log($"ACTOR NUMBER {actorNumber}");

        if (amount > 0) // Block to rest player's scores.
        {
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

            amount = 0;
            return;
        }

        if (actorNumber == 1)
        {
            scorePlayer1++;
            textScoreplayer1.text = $"{scorePlayer1}";
        }
        else if (actorNumber == 2)
        { 
            scorePlayer2++;
            textScoreplayer2.text = $"{scorePlayer2}";
        }
    }
}
