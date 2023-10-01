using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    public parametrs parametrs;

    public SpriteRenderer spriteRenderer;
    public LineRenderer lineRenderer;
    public Sprite[] sprites;

    public float coldawnDefolt = 1;
    private float coldawn;
    private bool IsSpawn = false;
    public LayerMask layerMask;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;

    private void OnEnable()
    {
        SystemEvent.EndStep += SetIsSpawn;
        SystemEvent.Upgrade += Upgrade;
    }
    private void OnDisable()
    {
        SystemEvent.EndStep -= SetIsSpawn;
        SystemEvent.Upgrade -= Upgrade;
    }
    private void Start()
    {
        spriteRenderer.sprite = sprites[0];
        coldawn = coldawnDefolt;
    }
    private void Upgrade()
    {
        if (parametrs.LvL <= sprites.Length)
            spriteRenderer.sprite = sprites[parametrs.LvL - 1];
    }
    private void Update()
    {
        if (!IsSpawn) return;
        hitUp = Physics2D.Raycast(transform.transform.position, Vector2.up, Mathf.Infinity, layerMask);
        hitDown = Physics2D.Raycast(transform.transform.position, -Vector2.up, Mathf.Infinity, layerMask);

        if (coldawn < 0.3f)
        {
            coldawn = coldawn - Time.deltaTime;

            if (hitUp.transform != null && hitUp.transform.gameObject.tag == "Asteroid")
            {
                lineRenderer.enabled = true;
                Fire(hitUp.transform.gameObject);
            }
            else if (hitDown.transform != null && hitDown.transform.gameObject.tag == "Asteroid")
            {
                lineRenderer.enabled = true;
                Fire(hitDown.transform.gameObject);
            }
            if (coldawn < 0)
                coldawn = coldawnDefolt;
        }
        else
        {
            coldawn = coldawn - Time.deltaTime;
            lineRenderer.enabled = false;
        }
    }
    private void Fire(GameObject direction)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, direction.transform.position);
        direction.GetComponent<Asteroid>().TakeDamage(parametrs.Damage[parametrs.LvL]);
    }
    private void SetIsSpawn(bool isSpawn)
    {
        IsSpawn = isSpawn;
    }
}
