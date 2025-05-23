using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    public parametrs parametrs;

    public SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererActive;
    public LineRenderer lineRenderer;
    public Sprite[] sprites;

    private float coldawn;
    private bool IsSpawn = false;
    public LayerMask layerMask;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    public Transform startFire;
    public AudioSource AudioLaser;

    private void OnEnable()
    {
        SystemEvent.EndStep += SetIsSpawn;
        SystemEvent.Upgrade += Upgrade;
        SystemEvent.StartStep += Setrotation;
        SystemEvent.MuteAudio += AudioMute;
        SystemEvent.PlayAudio += AudioPlay;
    }
    private void OnDisable()
    {
        SystemEvent.EndStep -= SetIsSpawn;
        SystemEvent.Upgrade -= Upgrade;
        SystemEvent.StartStep -= Setrotation;
        SystemEvent.MuteAudio -= AudioMute;
        SystemEvent.PlayAudio -= AudioPlay;
    }
    private void Start()
    {
        spriteRenderer.enabled = false;
        spriteRendererActive = GetComponent<SpriteRenderer>();
        coldawn = parametrs.Firerate[parametrs.LvL];
    }
    private void Upgrade()
    {
        if (parametrs.LvL <= sprites.Length)
            spriteRendererActive.sprite = sprites[parametrs.LvL - 1];
    }
    private void Update()
    {
        if (!IsSpawn) return;
        hitUp = Physics2D.Raycast(transform.transform.position, Vector2.up, 5, layerMask);
        hitDown = Physics2D.Raycast(transform.transform.position, -Vector2.up, 5, layerMask);

        if (coldawn < 0.3f)
        {
            coldawn = coldawn - Time.deltaTime;

            if (hitUp.transform != null && hitUp.transform.gameObject.tag == "Asteroid")
            {
                spriteRendererActive.flipY = false;
                lineRenderer.enabled = true;
                Fire(hitUp.transform.gameObject);
            }
            else if (hitDown.transform != null && hitDown.transform.gameObject.tag == "Asteroid")
            {
                spriteRendererActive.flipY = true;
                lineRenderer.enabled = true;
                Fire(hitDown.transform.gameObject);
            }
            if (coldawn < 0)
                coldawn = parametrs.Firerate[parametrs.LvL];
        }
        else
        {
            coldawn = coldawn - Time.deltaTime;
            lineRenderer.enabled = false;
        }
    }
    private void Fire(GameObject direction)
    {
        if (PlayerPrefs.HasKey("MuteAudio") == false)
            AudioLaser.Play();
        lineRenderer.SetPosition(0, startFire.position);
        lineRenderer.SetPosition(1, direction.transform.position);
        direction.GetComponent<Asteroid>().TakeDamage(parametrs.Damage[parametrs.LvL]);
    }
    private void SetIsSpawn(bool isSpawn)
    {
        IsSpawn = isSpawn;
    }
    private void Setrotation()
    {
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
    public void AudioMute()
    {
        AudioLaser.mute = true;
    }
    public void AudioPlay()
    {
        AudioLaser.mute = false;
    }
}
