using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortira : MonoBehaviour
{
    public parametrs parametrs;
    private bool IsSpawn = false;

    public SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererActive;
    public Sprite[] sprites;

    private List<GameObject> Asteroids;

    private float coldawn;

    public GameObject bulet;
    public Transform startFire;
    private GameObject Bulet;
    public AudioSource AudioMortira;

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
        spriteRenderer.enabled = false;
        spriteRendererActive = GetComponent<SpriteRenderer>();
        Asteroids = new List<GameObject>();
    }
    private void Update()
    {
        if (!IsSpawn) return;

        if (Asteroids.Count > 0)
        {
            if (coldawn < 0)
            {
                Fire(Asteroids[0]);
                coldawn = parametrs.Firerate[parametrs.LvL];
            }
            else
            {
                coldawn = coldawn - Time.deltaTime;
            }
        }
    }
    private void Fire(GameObject target)
    {
        if(transform.position.x - target.transform.position.x > 0)
        {
            spriteRendererActive.flipX = false;
            Bulet = Instantiate(bulet, startFire.position, Quaternion.identity);
        }
        else
        {
            spriteRendererActive.flipX = true;
            Bulet = Instantiate(bulet, startFire.position + Vector3.right, Quaternion.identity);
        }

        bulet buletScript = Bulet.GetComponent<bulet>();
        buletScript.target = target;
        buletScript.damage = parametrs.Damage[parametrs.LvL];
        //buletScript.explosionRadius = parametrs.radius[parametrs.LvL];
        buletScript.isMortira = true;
        AudioMortira.Play();
    }
    private void Upgrade()
    {
        if (parametrs.LvL <= sprites.Length)
            spriteRendererActive.sprite = sprites[parametrs.LvL - 1];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Asteroids.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Asteroids.Remove(collision.gameObject);
        }
    }
    private void SetIsSpawn(bool isSpawn)
    {
        IsSpawn = isSpawn;
    }
}
