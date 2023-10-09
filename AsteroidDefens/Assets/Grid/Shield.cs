using UnityEngine;

public class Shield : MonoBehaviour
{
    private Animation AnimShield;
    public AudioSource AudioShield;

    private void OnEnable()
    {
        SystemEvent.HitPlayer += Animated;
        SystemEvent.MuteAudio += AudioMute;
        SystemEvent.PlayAudio += AudioPlay;
    }
    private void OnDisable()
    {
        SystemEvent.HitPlayer -= Animated;
        SystemEvent.MuteAudio -= AudioMute;
        SystemEvent.PlayAudio -= AudioPlay;
    }
    private void Start()
    {
        AnimShield = GetComponent<Animation>();
    }
    private void Animated(GameObject Asteroid)
    {
        if (!AnimShield.isPlaying)
        {
            AnimShield.Play(); 
            AudioShield.Stop();
            AudioShield.Play();
        }
    }
    public void AudioMute()
    {
        AudioShield.mute = true;
    }
    public void AudioPlay()
    {
        AudioShield.mute = false;
    }
}
