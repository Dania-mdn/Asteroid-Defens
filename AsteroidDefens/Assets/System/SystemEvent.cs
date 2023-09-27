using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemEvent : MonoBehaviour
{
    public static event Action Upgrade;
    public static event Action Step;
    public static event Action<bool> EndStep;
    public static event Action<GameObject> HitPlayer;
    public static event Action EndGame;
    public static event Action StartStep;
    public static event Action FullStep;
    public static void DoUpgrade()
    {
        Upgrade?.Invoke();
    }
    public static void DoStep()
    {
        Step?.Invoke();
    }
    public static void DoEndStep(bool IsSpawns)
    {
        EndStep?.Invoke(IsSpawns);
    }
    public static void DoHitPlayer(GameObject Asteroid)
    {
        HitPlayer?.Invoke(Asteroid);
    }
    public static void DoEndGame()
    {
        EndGame?.Invoke();
    }
    public static void DoStartStep()
    {
        StartStep?.Invoke();
    }
    public static void DoFullStep()
    {
        FullStep?.Invoke();
    }
}
