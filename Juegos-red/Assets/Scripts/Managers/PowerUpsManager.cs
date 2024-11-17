using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPickup;

    [SerializeField] private int MercuryBombPointsDamage;

    [SerializeField] private bool isDoublePointsActive;

    public bool GetIsDoublePointsActive => isDoublePointsActive;

    public int GetMercuryBombDamage => MercuryBombPointsDamage;

    TimerManager timerManager;

    // Start is called before the first frame update
    void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.OnSpawnPowerUp += SpawnNextPowerUp;
        }

        powerUpPickup.SetActive(false);
    }

    private void SpawnNextPowerUp()
    {
        PhotonNetwork.Instantiate(powerUpPickup.name, powerUpPickup.transform.position, Quaternion.identity);
    }

    public void StateDoublePoints(PowerUp powerUp)
    {
        if (powerUp.powerUpName == "DoublePoints" && !isDoublePointsActive)
        {
            isDoublePointsActive = true;
        }
        else if (powerUp.powerUpName == "DoublePoints")
        {
            isDoublePointsActive = false;
        }
    }

    private void OnDestroy()
    {
        timerManager.OnSpawnPowerUp -= SpawnNextPowerUp;
    }
}
