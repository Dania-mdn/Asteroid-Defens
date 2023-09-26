using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemEvent : MonoBehaviour
{
    public static event Action Upgrade;
    public static event Action Step;
    public static void DoUpgrade()
    {
        Upgrade?.Invoke();
    }
    public static void DoStep()
    {
        Step?.Invoke();
    }
}
