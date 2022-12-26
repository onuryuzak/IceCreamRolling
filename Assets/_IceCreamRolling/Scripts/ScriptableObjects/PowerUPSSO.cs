using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PowerUpsSO", menuName = "GameData/PowerUpData/PowerUpsSO", order = 1)]
public class PowerUPSSO : ScriptableObject
{
    [EnumToggleButtons]
    public PowerUp PowerUpType;

    [ShowIf("@PowerUpType", PowerUp.Speed)]
    public float SpeedPowerUp;
    [ShowIf("@PowerUpType", PowerUp.Speed)]
    public int SpeedPowerUpPrice;

    [ShowIf("@PowerUpType", PowerUp.DiggingSpeed)]
    public float DiggingSpeedPowerUp;
    [ShowIf("@PowerUpType", PowerUp.DiggingSpeed)]
    public int DiggingSpeedPowerUpPrice;

    [ShowIf("@PowerUpType", PowerUp.IncomeMoney)]
    public int IncomeMoneyPowerUp;
    [ShowIf("@PowerUpType", PowerUp.IncomeMoney)]
    public int IncomeMoneyPowerUpPrice;


    
}

public enum PowerUp
{
    Speed,
    DiggingSpeed,
    IncomeMoney
}