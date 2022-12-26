using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamAreaManager : MonoBehaviour
{
    public IceCreamAreaPropertiesSO IceCreamAreaPropertiesSO;
    private IceCreamAreaController _iceCreamAreaController;
    private UnlockTriggerIceCreamAreaController _unlockTriggerIceCreamAreaController;

    private void Start()
    {
        _iceCreamAreaController = GetComponentInChildren<IceCreamAreaController>();
        _unlockTriggerIceCreamAreaController = GetComponentInChildren<UnlockTriggerIceCreamAreaController>();

        if (!IceCreamAreaPropertiesSO.isUnlocked)
        {
            _iceCreamAreaController.StartGameProcess(true);
            _unlockTriggerIceCreamAreaController.IsUnlockAreaWillOpen(true);
        }
        else
        {
            _iceCreamAreaController.StartGameProcess(false);
            _unlockTriggerIceCreamAreaController.IsUnlockAreaWillOpen(false);
        }
    }

    public void OpenNewIceCreamArea()
    {
        if (IceCreamAreaPropertiesSO.isUnlocked)
        {
            _iceCreamAreaController.IsIceCreamAreaWillOpen(true);
            _unlockTriggerIceCreamAreaController.IsUnlockAreaWillOpen(false);
        }
    }
}
