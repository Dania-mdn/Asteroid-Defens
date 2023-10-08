using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explousion : MonoBehaviour
{
    public AudioSource AudioExplosion;
    void Start()
    {
        AudioExplosion.Play();
        Destroy(gameObject, 2);
    }
}
