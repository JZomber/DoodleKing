using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManager : MonoBehaviour
{    
    [SerializeField] private Animator transition; //Transición entre escenas

    private TimerManager timerManager;
    
    // Start is called before the first frame update
    void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.OnGameFinished += LoadMenu;
        }
    }

    public void LoadMenu()
    {
        StartCoroutine(InitialTransition());
    }

    private IEnumerator InitialTransition()
    {
        yield return new WaitForSeconds(3f);

        transition.SetTrigger("Start");

        StartCoroutine(MenuScreen("ScreenMenu"));
    }

    private IEnumerator MenuScreen(string str) //Carga la pantalla "Menú"
    {
        yield return new WaitForSeconds(2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }
}
