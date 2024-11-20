using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class SceneManager : MonoBehaviour
{    
    [SerializeField] private Animator transition; // Transition between scenes

    private TimerManager timerManager;
    private GameplayCallBacks gameplayCallBacks;
    
    // Start is called before the first frame update
    void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.OnGameFinished += LoadPlayersScreens;
        }

        gameplayCallBacks = FindObjectOfType<GameplayCallBacks>();
        if (gameplayCallBacks != null)
        {
            gameplayCallBacks.OnMatchCanceled += LoadMenu;
        }
    }

    private void LoadPlayersScreens()
    {
        int winnerActorNumber = ScoreManager.instance.GetWinnerActorNumber();

        if (winnerActorNumber == -1)
        {
            LoadTieScreen();
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == winnerActorNumber)
        {
            LoadWinScreen();
        }
        else
        {
            LoadDefeatScreen();
        }

        timerManager.OnGameFinished -= LoadPlayersScreens;
        gameplayCallBacks.OnMatchCanceled -= LoadMenu;
    }

    private void LoadWinScreen()
    {
        InitialTransition("ScreenVictory");
    }

    private void LoadDefeatScreen()
    {
        InitialTransition("ScreenDefeat");
    }

    private void LoadTieScreen()
    {
       InitialTransition("ScreenTie");
    }

    private void LoadMenu() // Usually when a player disconnects
    {
        StartCoroutine(LoadSceneWithDelay("ScreenMenu"));

        timerManager.OnGameFinished -= LoadPlayersScreens;
        gameplayCallBacks.OnMatchCanceled -= LoadMenu;
    }

    private void InitialTransition(string sceneName)
    {
        transition.SetTrigger("Start");

        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string str)
    {
        yield return new WaitForSeconds(3f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }

    private IEnumerator LoadSceneWithDelay(string str)
    {   
        yield return new WaitForSeconds(4f);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }
}
