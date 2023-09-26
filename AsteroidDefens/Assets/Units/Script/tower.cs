using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour
{
    public parametrs parametrs;

    private SpriteRenderer spriteRenderer;
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
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    private void Upgrade()
    {
        if (parametrs.LvL <= sprites.Length)
            spriteRenderer.sprite = sprites[parametrs.LvL - 1];
    }
}
