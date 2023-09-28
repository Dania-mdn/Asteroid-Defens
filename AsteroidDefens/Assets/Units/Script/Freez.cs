using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freez : MonoBehaviour
{
    public parametrs parametrs;

    public SpriteRenderer spriteRenderer;
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
        spriteRenderer.sprite = sprites[0];
    }

    private void Upgrade()
    {
        if(parametrs.LvL <= sprites.Length)
        {
            spriteRenderer.sprite = sprites[parametrs.LvL - 1];
        }
    }
}
