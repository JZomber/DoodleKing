using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator transition;

    public void LoadMenu()
    {
        StartCoroutine(MenuScreen("ScreenMenu"));
    }

    public void LoadLobby()
    {
        StartCoroutine(LobbyScreen("ScreenLobby"));
    }
    
    public void LoadNextLevel() //Carga el primer nivel
    {
        StartCoroutine(LoadLevel("Level_1"));
        
        //StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    
    private IEnumerator MenuScreen(string str) //Carga la pantalla "Menú"
    {
        ActivateTransition();

        yield return new WaitForSeconds(2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }

    private IEnumerator LoadLevel(string str)
    {
        ActivateTransition();

        yield return new WaitForSeconds(2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }
    
    private IEnumerator LobbyScreen(string str)
    {
        ActivateTransition();

        yield return new WaitForSeconds(2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }
    
    public void GameQuit() // Quita el juego
    {
        Application.Quit();
    }

    public void ActivateTransition()
    {
        transition.SetTrigger("Start");
    }
}
