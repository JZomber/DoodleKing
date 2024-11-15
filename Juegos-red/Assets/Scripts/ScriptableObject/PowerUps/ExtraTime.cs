using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExtraTime", menuName = "PowerUp/ExtraTime")]
public class ExtraTime : PowerUp
{
    public float extraTimeAmount = 30f;

    public override void ActivatePowerUp(GameObject player)
    {
        base.ActivatePowerUp(player);

        TimerManager timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.ExtraTime(this, extraTimeAmount);
        }
    }
}
