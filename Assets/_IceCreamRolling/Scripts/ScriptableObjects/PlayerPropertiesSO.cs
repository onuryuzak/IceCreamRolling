using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPropertiesSO", menuName = "GameData/PlayerData/PlayerPropertiesSO", order = 1)]
public class PlayerPropertiesSO : ScriptableObject
{
    public int Money;
    public int moneyDecreaseFactor = 1;

    public float MovementSpeed;
    public float DiggingSpeed;
    public int IncomeIncreaseFactor;
}
