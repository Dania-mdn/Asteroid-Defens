using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class step : MonoBehaviour
{
    public parametrs parametrs;

    public SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererActive;
    public Sprite[] sprites;
    public Sprite spritesOpen;
    public bool IsOpen = false;

    private void OnEnable()
    {
        SystemEvent.Upgrade += Upgrade;
        SystemEvent.CloseStep += SetClosen;
    }
    private void OnDisable()
    {
        SystemEvent.Upgrade -= Upgrade;
        SystemEvent.CloseStep -= SetClosen;
    }
    private void Start()
    {
        spriteRenderer.enabled = false;
        spriteRendererActive = GetComponent<SpriteRenderer>();
    }

    private void Upgrade()
    {
        if (parametrs.LvL <= sprites.Length)
            spriteRendererActive.sprite = sprites[parametrs.LvL - 1];
    }
    public void SetOpen()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            spriteRendererActive.sprite = spritesOpen;
        }
        else
        {
            AddStep();
        }
    }
    public void SetClosen()
    {
        IsOpen = false;
        spriteRendererActive.sprite = sprites[parametrs.LvL - 1];
    }
    private void AddStep()
    {
        SystemEvent.DoAddStep(parametrs.Step[parametrs.LvL]);
    }
}
