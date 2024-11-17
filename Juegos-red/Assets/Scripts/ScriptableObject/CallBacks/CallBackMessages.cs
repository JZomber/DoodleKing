using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCallBackMessages", menuName = "CallBacks/Messages")]
public class CallBackMessages : ScriptableObject
{
    public string OnPlayerDisconnected = "Un jugador se ha desconectado de la partida. Volviendo al menu...";
    public string OnJoinRoomFailed = "El nombre de la sala no es correcto o la sala no existe.";
    public string OnNullName = "Debe ingresar el nombre de la sala.";
    public string OnTryToJoinFullRoom = "La sala que intenta ingresar se encuentra llena.";
    public string OnTryToJoinWithGameVersionMismatch = "Fallo al unirse, el juego se encuentra desactualizado.";
}
