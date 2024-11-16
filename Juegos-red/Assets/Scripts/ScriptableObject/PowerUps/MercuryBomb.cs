using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMercuryBomb", menuName = "PowerUp/MercuryBomb")]
public class MercuryBomb : PowerUp
{
    public int pointsDamage;

    public override void ActivatePowerUp(GameObject player)
    {
        base.ActivatePowerUp(player);

        PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.MercuryBombPowerUp(this);
        }
        else
        {
            Debug.LogWarning($"El componente (PlayerCombat) del jugador {player}, no ha sido encontrado");
        }
    }


}
