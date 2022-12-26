using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void OnGetNewCostumerDelegate();
    public static event OnGetNewCostumerDelegate OnNewCostumer;

    public static void NewCustomer()
    {
        OnNewCostumer?.Invoke();
    }
    public delegate void OnTickDelegate(bool state);
    public static event OnTickDelegate OnTick;

    public static void Tick(bool state)
    {
        OnTick?.Invoke(state);
    }
}
