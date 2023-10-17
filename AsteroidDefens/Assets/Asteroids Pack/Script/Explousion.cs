using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explousion : MonoBehaviour
{
    public AudioSource AudioExplosion;
    private void OnEnable()
    {
        SystemEvent.MuteAudio += AudioMute;
        SystemEvent.PlayAudio += AudioPlay;
    }
    private void OnDisable()
    {
        SystemEvent.MuteAudio -= AudioMute;
        SystemEvent.PlayAudio -= AudioPlay;
    }
    void Start()
    {
        if(PlayerPrefs.HasKey("MuteAudio") == false)
            AudioExplosion.Play();

        Destroy(gameObject, 2);
    }
    public void AudioMute()
    {
        AudioExplosion.mute = true;
    }
    public void AudioPlay()
    {
        AudioExplosion.mute = false;
    }
}
