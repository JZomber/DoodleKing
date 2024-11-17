using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMPro.TMP_InputField createInput; //Room's name to create
    [SerializeField] private TMPro.TMP_InputField joinInput; // Room's name to join

    [SerializeField] private GameObject CallBackCanvas;
    [SerializeField] private TMPro.TMP_Text errorText;
    [SerializeField] private CallBackMessages callBackMessages; // S.Object with different messages

    [SerializeField] private List<GameObject> deactivableObjects;

    private MenuManager menuManager;
    
    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
    }

    private void OnDestroy()
    {
        createButton.onClick.RemoveAllListeners();
        joinButton.onClick.RemoveAllListeners();
    }

    private void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInput.text))
        {
            CaseErrorMessage(ErrorType.NullName);
            return;
        }

        menuManager.ActivateTransition();

        RoomOptions roomConfiguration = new RoomOptions();
        roomConfiguration.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomConfiguration);
    }

    private void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinInput.text))
        {
            CaseErrorMessage(ErrorType.NullName);
            return;
        }

        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        menuManager.ActivateTransition();
        
        PhotonNetwork.LoadLevel("Level_1"); // Gameplay level
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        switch (returnCode)
        {
            case Photon.Chat.ErrorCode.GameDoesNotExist:
                CaseErrorMessage(ErrorType.RoomDoesntExist);
            break;

            case Photon.Chat.ErrorCode.GameFull:
                CaseErrorMessage(ErrorType.FullRoom);
            break;
        }
    }

    private void CaseErrorMessage(ErrorType errorType)
    {
        switch (errorType)
        {
            case ErrorType.NullName:
                ActivateCallback(callBackMessages.OnNullName);
            break;

            case ErrorType.RoomDoesntExist:
                ActivateCallback(callBackMessages.OnJoinRoomFailed);
            break;

            case ErrorType.FullRoom:
                ActivateCallback(callBackMessages.OnTryToJoinFullRoom);
            break;

            default:
                Debug.LogWarning("No se encontró un mensaje para el error.");
            break;
        }
    }

    private void ActivateCallback(string message)
    {
        foreach (var menuObject in deactivableObjects)
        {
            menuObject.SetActive(false);
        }
        
        CallBackCanvas.SetActive(true);
        errorText.text = message;
    }
}

public enum ErrorType
{
    PlayerDisconnected,
    RoomDoesntExist,
    NullName,
    FullRoom,
    OutdatedVersion,
}