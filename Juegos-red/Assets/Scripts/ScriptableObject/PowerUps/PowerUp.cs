using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUp/PowerUpSO")]
public class PowerUp : ScriptableObject
{
    public string powerUpName = "New PowerUp";

    public Sprite powerUpIcon;

    public bool hasDuration;
    public float duration = 1.0f;

    public virtual void ActivatePowerUp(GameObject player)
    {
        Debug.Log("Activando power up: " + powerUpName);
    }

    public virtual void DeactivatePowerUp(GameObject player)
    {
        Debug.Log("Power up desactivado: " + powerUpName);
    }
}
