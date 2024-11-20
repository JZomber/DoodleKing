using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CaptureZone : MonoBehaviourPun
{
    private bool playerInsideZone;
    private GameObject playerGameObject;
    private Coroutine pointsCoroutine; // Coroutine instance || Each "StartCoroutine" is an instance by itself. Better look for an individual variable to store singular coroutines.
    
    private Collider2D Collider2D;
    private SpriteRenderer spriteRenderer;

    private GameplayCallBacks gameplayCallBacks;
    private TimerManager timerManager;
    private PowerUpsManager powerUpsManager;

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Collider2D.enabled = false;
        spriteRenderer.enabled = false;

        gameplayCallBacks = FindObjectOfType<GameplayCallBacks>();
        if (gameplayCallBacks != null)
        {
            gameplayCallBacks.OnMatchBeging += InitializeRPC;
            gameplayCallBacks.OnMatchCanceled += DeactivateZone;
        }

        timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.OnGameFinished += DeactivateZone;
        }

        powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInsideZone = true;
            playerGameObject = collision.gameObject;
            ChangeColor(true);

            if (pointsCoroutine == null)
            {
                pointsCoroutine = StartCoroutine(AddPlayerPointsEverySecond());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInsideZone = false;
            playerGameObject = null;
            ChangeColor(false);

            if (pointsCoroutine != null)
            {
                StopCoroutine(pointsCoroutine);
                pointsCoroutine = null;
            }
        }
    }

    // Changes Zone's color
    private void ChangeColor(bool playerDetected)
    {
        GetComponent<Renderer>().material.color = playerDetected ? Color.green : Color.red;
    }

    // Delay before adding points to the player
    private IEnumerator AddPlayerPointsEverySecond()
    {
        while (playerInsideZone)
        {
            yield return new WaitForSeconds(1f);

            if (playerGameObject != null)
            {
                PhotonView playerPV = playerGameObject.GetComponent<PhotonView>();

                if (playerPV != null && playerPV.IsMine)
                {
                    int amount = 1;

                    if (powerUpsManager.GetIsDoublePointsActive)
                    {
                        amount = 2;
                    }

                    ScoreManager.instance.AddScorePoints(playerPV.OwnerActorNr, amount);
                }
            }
        }

        pointsCoroutine = null;
    }

    private void InitializeRPC()
    {
        photonView.RPC("InitializeCaptureZone", RpcTarget.All);
    }

    [PunRPC]
    private void InitializeCaptureZone()
    {
        Collider2D.enabled = true;
        spriteRenderer.enabled = true;

        gameplayCallBacks.OnMatchBeging -= InitializeRPC;
    }

    private void DeactivateZone()
    {
        Collider2D.enabled = false;
        spriteRenderer.enabled = false;

        if (pointsCoroutine != null)
        {
            StopCoroutine(pointsCoroutine);
            pointsCoroutine = null;
        }

        timerManager.OnGameFinished -= DeactivateZone;
        gameplayCallBacks.OnMatchCanceled -= DeactivateZone;
    }
}
