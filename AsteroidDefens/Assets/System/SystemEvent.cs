using System;
using UnityEngine;

public class SystemEvent : MonoBehaviour
{
    public static event Action Upgrade;
    public static event Action Step;
    public static event Action<bool> EndStep;
    public static event Action<GameObject> HitPlayer;
    public static event Action AddHealth;
    public static event Action EndGame;
    public static event Action StartStep;
    public static event Action FullStep;
    public static event Action<GameObject> DestroyAsteroid;
    public static event Action<int> AddStep;
    public static event Action CloseStep;
    public static event Action<float> SpawnBoss;
    public static event Action DestroyBoss;
    public static event Action<float> HitBoss;
    public static event Action MuteAudio;
    public static event Action PlayAudio;
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
    public static void DoAddHealth()
    {
        AddHealth?.Invoke();
    }
    public static void DoDestroyAsteroid(GameObject Asteroid)
    {
        DestroyAsteroid?.Invoke(Asteroid);
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
    public static void DoAddStep(int step)
    {
        AddStep?.Invoke(step);
    }
    public static void DoCloseStep()
    {
        CloseStep?.Invoke();
    }
    public static void DoSpawnBoss(float hp)
    {
        SpawnBoss?.Invoke(hp);
    }
    public static void DoDestroyBoss()
    {
        DestroyBoss?.Invoke();
    }
    public static void DoHitBoss(float Hp)
    {
        HitBoss?.Invoke(Hp);
    }
    public static void DoMuteAudio()
    {
        MuteAudio?.Invoke();
    }
    public static void DoPlayAudio()
    {
        PlayAudio?.Invoke();
    }
}
