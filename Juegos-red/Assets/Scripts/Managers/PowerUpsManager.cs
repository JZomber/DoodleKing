using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField] private int MercuryBombPointsDamage;

    [SerializeField] private bool isDoublePointsActive;

    public bool GetIsDoublePointsActive => isDoublePointsActive;

    public int GetMercuryBombDamage => MercuryBombPointsDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
