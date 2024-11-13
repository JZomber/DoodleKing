using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class GameplayCallBacks : MonoBehaviourPunCallbacks
{
    public event Action OnMatchBeging; 

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnMatchBeging?.Invoke();
        Debug.Log("Nuevo jugador ha entrado a la partida");
        //base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Un jugador ha salido de la partida");
        //base.OnPlayerLeftRoom(otherPlayer);
    }
}
