using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaPropertiesSO", menuName = "GameData/IceCreamAreaData/FreezeAreaPropertiesSO", order = 1)]
public class IceCreamAreaPropertiesSO : ScriptableObject
{
    public Material AreaIceCreamMaterial;
    public Material DrawMaterial;
    public int IceCreamUnlockFee;
    public bool isUnlocked;
}
