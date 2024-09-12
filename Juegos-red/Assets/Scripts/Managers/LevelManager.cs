using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player; //Referencia al player
    private PlayerDamage _playerDamage;
    
    [SerializeField] private GameObject mainCamera; //Referencia a la camara
    
    [SerializeField] private GameObject holderPlayerWaypoints; //Objeto que almacena las pocisiones a donde "teletransportar" al player
    [SerializeField] private Transform[] playerWaypoints; //Array de posiciones
    private int _totalPlayerWaypoints;
    private int _indexPlayerWaypoint;
    
    [SerializeField] private Animator transition; //Transición entre escenas
    
    // Start is called before the first frame update
    void Start()
    {
        if (holderPlayerWaypoints != null)
        {
            _totalPlayerWaypoints = holderPlayerWaypoints.transform.childCount;
            playerWaypoints = new Transform[_totalPlayerWaypoints];

            for (int i = 0; i < _totalPlayerWaypoints; i++)
            {
                playerWaypoints[i] = holderPlayerWaypoints.transform.GetChild(i).transform;
            } 
        }
    }
    
    private IEnumerator VictoryScreen(float delay) //Pantalla de victoria
    {
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ScreenVictory");
    }
    
    private IEnumerator DefeatScreen(float delay) //Pantalla de derrota
    {
        yield return new WaitForSeconds(delay);
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ScreenDefeat");
    }
}
