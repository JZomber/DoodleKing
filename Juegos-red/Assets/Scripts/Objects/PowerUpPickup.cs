using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpPickup : MonoBehaviourPun
{
    [SerializeField] PowerUp[] powerUps;
    private SpriteRenderer spriteRenderer;
    private int selectedPowerUpIndex;
    private Collider2D Collider2D;
    private Coroutine coroutineSelfDestroy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomIndex = Random.Range(0, powerUps.Length);

            photonView.RPC("SelectRandomPowerUp", RpcTarget.All, randomIndex);

            if (coroutineSelfDestroy == null)
            {
                coroutineSelfDestroy = StartCoroutine(SelfDestroy());
            }
        }
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(8f);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (powerUps[selectedPowerUpIndex].hasDuration)
            {
                StartCoroutine(ActivatePowerUpWithDuration(collision.gameObject));
            }
            else
            {
                ActivatePowerUpEffect(collision.gameObject);
            }

            photonView.RPC("DisablePowerUp", RpcTarget.All);
        }
    }

    private void ActivatePowerUpEffect(GameObject player)
    {
        powerUps[selectedPowerUpIndex].ActivatePowerUp(player);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator ActivatePowerUpWithDuration(GameObject player)
    {
        powerUps[selectedPowerUpIndex].ActivatePowerUp(player);

        yield return new WaitForSeconds(powerUps[selectedPowerUpIndex].duration);

        powerUps[selectedPowerUpIndex].DeactivatePowerUp(player);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    [PunRPC]
    private void SelectRandomPowerUp(int index)
    {
        if (powerUps.Length == 0)
        {
            Debug.LogWarning("No hay power ups asignados al array del objeto");
            return;
        }

        selectedPowerUpIndex = index;

        spriteRenderer.sprite = powerUps[selectedPowerUpIndex].powerUpIcon;
    }

    [PunRPC]
    private void DisablePowerUp()
    {

        spriteRenderer.enabled = false;
        Collider2D.enabled = false;

        if (PhotonNetwork.IsMasterClient && coroutineSelfDestroy != null)
        {
            StopCoroutine(coroutineSelfDestroy);
            coroutineSelfDestroy = null;
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);

        PhotonNetwork.Destroy(gameObject);
    }
}
