using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CaptureZone : MonoBehaviour
{
    private bool playerInsideZone;
    private GameObject playerGameObject;
    private Coroutine pointsCoroutine; // Coroutine instance || Each "StartCoroutine" is an instance by itself. Better look for an individual variable to store singular coroutines.

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
                    ScoreManager.instance.AddScorePoints(playerPV.OwnerActorNr);
                }
            }
        }

        pointsCoroutine = null;
    }
}
