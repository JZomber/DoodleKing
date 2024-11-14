using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] PowerUp[] powerUps;


    private SpriteRenderer spriteRenderer;
    private int selectedPowerUpIndex;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SelectRandomPowerUp();
    }

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
        powerUps[selectedPowerUpIndex].ActivatePowerUp(player);

        yield return new WaitForSeconds(powerUps[selectedPowerUpIndex].duration);

        powerUps[selectedPowerUpIndex].DeactivatePowerUp(player);
    }

    private void SelectRandomPowerUp()
    {
        if (powerUps.Length == 0)
        {
            Debug.LogWarning("No hay power ups asignados al array del objeto");
            return;
        }
    }
}
