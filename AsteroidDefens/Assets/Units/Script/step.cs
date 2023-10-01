using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class step : MonoBehaviour
{
    public parametrs parametrs;
    public Match3Control Match3Control;

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
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
        spriteRenderer.sprite = sprites[0];
    }

    private void Upgrade()
    {
        if (parametrs.LvL <= sprites.Length)
            spriteRenderer.sprite = sprites[parametrs.LvL - 1];
    }
    public void SetOpen()
    {
        if (!IsOpen)
        {
            IsOpen = true;
        }
        else
        {
            AddStep();
        }
    }
    public void SetClosen()
    {
        IsOpen = false;
    }
    private void AddStep()
    {
        SystemEvent.DoAddStep(parametrs.Step[parametrs.LvL]);
    }
}
