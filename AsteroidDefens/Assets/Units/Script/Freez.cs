using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freez : MonoBehaviour
{
    public parametrs parametrs;

    public SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererActive;
    public Sprite[] sprites;

    private void OnEnable()
    {
        SystemEvent.Upgrade += Upgrade;
    }
    private void OnDisable()
    {
        SystemEvent.Upgrade -= Upgrade;
    }
    private void Start()
    {
        spriteRenderer.enabled = false;
        spriteRendererActive = GetComponent<SpriteRenderer>();
    }

    private void Upgrade()
    {
        if(parametrs.LvL <= sprites.Length)
        {
            spriteRendererActive.sprite = sprites[parametrs.LvL - 1];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            asteroid.speed = parametrs.Freez[parametrs.LvL];
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            asteroid.speed = asteroid.Defoltspeed;
        }
    }
}
