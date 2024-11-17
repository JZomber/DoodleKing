using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] PowerUp[] powerUps;


    private SpriteRenderer spriteRenderer;
    private int selectedPowerUpIndex;
    private Collider2D Collider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        SelectRandomPowerUp();

        spriteRenderer.sprite = powerUps[selectedPowerUpIndex].powerUpIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ActivatePowerUpWithDuration(collision.gameObject));
        }

        spriteRenderer.enabled = false;
        Collider2D.enabled = false;
    }

    private IEnumerator ActivatePowerUpWithDuration(GameObject player)
    {
        powerUps[selectedPowerUpIndex].ActivatePowerUp(player);

        yield return new WaitForSeconds(powerUps[selectedPowerUpIndex].duration);

        powerUps[selectedPowerUpIndex].DeactivatePowerUp(player);

        Destroy(gameObject);
    }

    private void SelectRandomPowerUp()
    {
        if (powerUps.Length == 0)
        {
            Debug.LogWarning("No hay power ups asignados al array del objeto");
            return;
        }

        selectedPowerUpIndex = Random.Range(0, powerUps.Length);

        spriteRenderer.sprite = powerUps[selectedPowerUpIndex].powerUpIcon;
    }

    private void OnEnable()
    {
        SelectRandomPowerUp();

        spriteRenderer.sprite = powerUps[selectedPowerUpIndex].powerUpIcon;
    }
}
