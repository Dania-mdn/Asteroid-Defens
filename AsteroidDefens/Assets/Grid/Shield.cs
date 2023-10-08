using UnityEngine;

public class Shield : MonoBehaviour
{
    private Animation AnimShield;
    public AudioSource AudioShield;

    private void OnEnable()
    {
        SystemEvent.HitPlayer += Animated;
    }
    private void OnDisable()
    {
        SystemEvent.HitPlayer -= Animated;
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
}
