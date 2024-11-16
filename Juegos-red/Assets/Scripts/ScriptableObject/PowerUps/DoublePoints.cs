using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDoublePoints", menuName = "PowerUp/DoublePoints")]
public class DoublePoints : PowerUp
{
    PowerUpsManager powerUpsManager;

    public override void ActivatePowerUp(GameObject player)
    {
        base.ActivatePowerUp(player);

        powerUpsManager = FindObjectOfType<PowerUpsManager>();
        if (powerUpsManager != null)
        {
            powerUpsManager.StateDoublePoints(this);
        }
    }

    public override void DeactivatePowerUp(GameObject player)
    {
        base.DeactivatePowerUp(player);

        powerUpsManager.StateDoublePoints(this);
    }
}
