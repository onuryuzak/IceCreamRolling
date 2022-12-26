using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceMachineManager : MonoBehaviour
{
    public SauceMachineSO SauceMachineSO;
    private SauceMachineController _sauceMachineController;
    private SauceMachineUnlockTriggerAreaController _sauceMachineUnlockTriggerAreaController;

    private void Start()
    {
        _sauceMachineController = GetComponentInChildren<SauceMachineController>();
        _sauceMachineUnlockTriggerAreaController = GetComponentInChildren<SauceMachineUnlockTriggerAreaController>();

        if (!SauceMachineSO.isUnlocked)
        {
            _sauceMachineController.StartGameProgress(false);
            _sauceMachineUnlockTriggerAreaController.IsUnlockAreaWillOpen(true);
        }
        else
        {
            _sauceMachineController.StartGameProgress(true);
            _sauceMachineUnlockTriggerAreaController.IsUnlockAreaWillOpen(false);
        }
    }

    public void OpenNewSauceMachineArea()
    {
        if (SauceMachineSO.isUnlocked)
        {
            _sauceMachineController.IsSauceMachineWillOpen(true);
            _sauceMachineUnlockTriggerAreaController.IsUnlockAreaWillOpen(false);
        }
    }
}
