using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "SauceMachineSO", menuName = "GameData/SauceMachineData/SauceMachineSO", order = 1)]
public class SauceMachineSO : ScriptableObject
{
    [EnumToggleButtons]
    public SauceMachine SauceMachineType;

    [ShowIf("@SauceMachineType", SauceMachine.Strawberry)]
    public int StrawberrySauceUnlockFee;

    [ShowIf("@SauceMachineType", SauceMachine.Chocolate)]
    public int ChocolateSauceUnlockFee;

    [ShowIf("@SauceMachineType", SauceMachine.Caramel)]
    public int CaramelSauceUnlockFee;

    [ShowIf("@SauceMachineType", SauceMachine.Sprinkles)]
    public int SprinklesSauceUnlockFee;

    [ShowIf("@SauceMachineType", SauceMachine.PeanutParticle)]
    public int PeanutParticleSauceUnlockFee;

    public bool isUnlocked;
}

public enum SauceMachine
{
    Strawberry,
    Chocolate,
    Caramel,
    Sprinkles,
    PeanutParticle

}
