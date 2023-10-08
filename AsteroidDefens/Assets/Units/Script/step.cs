using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class step : MonoBehaviour
{
    public parametrs parametrs;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererActive;
    public Sprite[] sprites;
    public Sprite[] spritesOpen;
    public bool IsOpen = false;
    public GameObject text;
    public TextMeshProUGUI addStep;

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
            text.SetActive(true);
            addStep.text = parametrs.Step[parametrs.LvL].ToString();
            IsOpen = true;
            spriteRendererActive.sprite = spritesOpen[parametrs.LvL - 1];
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
        text.SetActive(false);
    }
    private void AddStep()
    {
        SystemEvent.DoAddStep(parametrs.Step[parametrs.LvL]);
    }
}
