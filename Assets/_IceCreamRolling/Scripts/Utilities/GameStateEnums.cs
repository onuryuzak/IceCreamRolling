using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEnums : Singleton<GameStateEnums>
{
    public static GameState currentGameState;
    public enum GameState
    {
        InIceCreamArea,
        InPowerUpArea,
        InSauceMachineArea,
        InConeOrCupArea,
        InCostumersArea,
        PlayerCanMove,
        InUnlockArea,
        Waiting,
        Play,
        Fail,
        Success,
    }
}
