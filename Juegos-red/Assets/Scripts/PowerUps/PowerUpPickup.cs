using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] PowerUp powerUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ActivatePowerUpWithDuration(collision.gameObject));

            gameObject.SetActive(false);
        }
    }

    private IEnumerator ActivatePowerUpWithDuration(GameObject player)
    {
        powerUp.ActivatePowerUp(player);

        yield return new WaitForSeconds(powerUp.duration);

        powerUp.DeactivatePowerUp(player);
    }
}
