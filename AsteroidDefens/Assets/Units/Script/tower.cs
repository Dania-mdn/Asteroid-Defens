using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour
{
    public parametrs parametrs;
    private bool IsSpawn = false;

    public SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererActive;
    public Sprite[] sprites;

    private List<GameObject> Asteroids;

    private float coldawn;

    public GameObject bulet;

    public Transform[] firepont0;
    private float angle;
    private Vector2 dir;
    public AudioSource AudioTower;

    private void OnEnable()
    {
        SystemEvent.EndStep += SetIsSpawn;
        SystemEvent.Upgrade += Upgrade;
        SystemEvent.StartStep += Setrotation;
    }
    private void OnDisable()
    {
        SystemEvent.EndStep -= SetIsSpawn;
        SystemEvent.Upgrade -= Upgrade;
        SystemEvent.StartStep -= Setrotation;
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

        if(Asteroids.Count > 0)
        {
            dir = Asteroids[0].transform.position - this.transform.position;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
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
        int j = Random.Range(0, 1);
        GameObject Bulet = Instantiate(bulet, firepont0[j].position, Quaternion.identity);
        bulet buletScript = Bulet.GetComponent<bulet>();
        buletScript.target = target;
        buletScript.damage = parametrs.Damage[parametrs.LvL];
        if (PlayerPrefs.HasKey("MuteAudio") == false)
            AudioTower.Play();
    }
    private void Upgrade()
    {
        if (parametrs.LvL <= sprites.Length)
            spriteRendererActive.sprite = sprites[parametrs.LvL - 1];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
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
        if(!IsSpawn)
            transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
    private void Setrotation()
    {
         transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
}
