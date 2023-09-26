using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemEvent : MonoBehaviour
{
    public static event Action Upgrade;
    public static event Action Step;
    public static event Action EndStep;
    public static event Action HitPlayer;
    public static event Action EndGame;
    public static void DoUpgrade()
    {
        Upgrade?.Invoke();
    }
    public static void DoStep()
    {
        Step?.Invoke();
    }
    public static void DoEndStep()
    {
        EndStep?.Invoke();
    }
    public static void DoHitPlayer()
    {
        HitPlayer?.Invoke();
    }
    public static void DoEndGame()
    {
        EndGame?.Invoke();
    }
}
