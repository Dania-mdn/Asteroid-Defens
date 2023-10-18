using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidController : MonoBehaviour
{
    private int width = 6;
    private float size = 0.8f;
    private Vector2[] widtharray;

    public float coldawn = 0.2f;
    private float timer;
    public int asteroidCount = 4;
    private int UpdateAsteroidCount = 3;
    private int SpawnAsteroidCount;
    private bool IsSpawn = false;
    private bool endGame = false;
    public int AttackCount = 1;

    public GameObject BigAsteroid;
    private int BigAsteroidDirectionl;
    public GameObject Asteroid;
    public List<GameObject> Asteroids;
    public GameObject BossDirection;
    private GameObject BossDirectionMediate;
    public ParticleSystem ParticleSystem;
    public Animation Animation;
    public GameObject plane;
    public GameObject Decor;
    public AudioSource DefoltAudio;
    public AudioSource FightAudio;
    public AudioSource Spaceship;

    public float coldawnStar = 15;
    private float timerStar;
    public Animation[] star;

    public GameObject Alliance;
    public Animation AnimationAlliance;
    private Image ImageAlliance;
    public Sprite[] SpriteAlliance;
    public float timerAlliance;

    private int AdsCount;
    public GameManager GameManager;

    private void OnEnable()
    {
        SystemEvent.EndStep += SetIsSpawn;
        SystemEvent.DestroyAsteroid += SetList;
        SystemEvent.HitPlayer += SetList;
        SystemEvent.EndGame += EndGame;
        SystemEvent.MuteAudio += AudioMute;
        SystemEvent.PlayAudio += AudioPlay;
    }
    private void OnDisable()
    {
        SystemEvent.EndStep -= SetIsSpawn;
        SystemEvent.DestroyAsteroid -= SetList;
        SystemEvent.HitPlayer -= SetList;
        SystemEvent.EndGame -= EndGame;
        SystemEvent.MuteAudio -= AudioMute;
        SystemEvent.PlayAudio -= AudioPlay;
    }
    private void Start()
    {
        DefoltAudio.Play();
        ParticleSystem.Stop();

        float posX = -size * width / 2f - size / 2f;

        widtharray = new Vector2[width];

        for(int i = 0; i < widtharray.Length; i++)
        {
            posX += size;
            widtharray[i] = new Vector2(posX, transform.position.y);
        }

        timer = coldawn;

        ImageAlliance = Alliance.GetComponent<Image>(); 
        AddStep();
    }
    private void Update()
    {

        if (timerStar > 0)
        {
            timerStar = timerStar - Time.deltaTime;
        }
        else
        {
            int i = Random.Range(0, star.Length);
            star[i].Play();
            timerStar = coldawnStar;
        }



        if (!IsSpawn)
        {
            if (timerAlliance > 0)
            {
                timerAlliance = timerAlliance - Time.deltaTime;
            }
            else
            {
                if (AnimationAlliance.isPlaying == false)
                {
                    AnimationAlliance.Play();
                    Alliance.SetActive(true);
                    ImageAlliance.sprite = SpriteAlliance[Random.Range(0, SpriteAlliance.Length)];
                }
                AddStep();
            }
        }
        else
        {
            if (AttackCount % 10 == 0)
            {
                GameObject MediateAsteroid;
                MediateAsteroid = Instantiate(BigAsteroid, widtharray[BigAsteroidDirectionl], Quaternion.identity, transform);
                MediateAsteroid.GetComponent<Asteroid>().asteroidController = this;
                MediateAsteroid.GetComponent<Asteroid>().Helth = asteroidCount;
                SetIsSpawn(false);
                AttackCount++;
            }
            else
            {
                if (timer < 0)
                {
                    GeneretedAsteroid();
                    timer = coldawn;
                }
                else
                {
                    timer = timer - Time.deltaTime;
                }
            }
        }
    }
    public void AddStep()
    {
        timerAlliance = Random.Range(40, 120);
    }
    private void GeneretedAsteroid()
    {
        int j = Random.Range(0, widtharray.Length); 
        GameObject MediateAsteroid;
        MediateAsteroid = Instantiate(Asteroid, widtharray[j], Quaternion.identity, transform);
        Asteroids.Add(MediateAsteroid);
        SpawnAsteroidCount++;

        if (SpawnAsteroidCount == asteroidCount)
        {
            SetIsSpawn(false);
            SpawnAsteroidCount = 0;
            asteroidCount = asteroidCount + UpdateAsteroidCount;
            AttackCount++;
            BigAsteroidDirectionl = Random.Range(0, widtharray.Length);
        }
    }
    private void SetIsSpawn(bool isSpawn)
    {
        IsSpawn = isSpawn;
        if (isSpawn)
        {
            if (BossDirectionMediate)
                Destroy(BossDirectionMediate);

            if(plane.activeSelf)
            {
                ParticleSystem.Play();
                Animation.Play();
                plane.SetActive(false);
                Decor.SetActive(true);
                DefoltAudio.Stop();
                FightAudio.Play();
                Spaceship.Play();
            }
            AdsCount++;
        }
    }
    private void SetList(GameObject Asteroid)
    {
        Asteroids.Remove(Asteroid);
            building();
    }
    public void building()
    {
        if (Asteroids.Count == 0 && !endGame)
        {
            if(AdsCount == 2)
            {
                AdsCount = 0;
                GameManager.ShowAd();
            }
            if (!plane.activeSelf)
            {
                ParticleSystem.Stop();
                Animation.Stop();
                plane.SetActive(true);
                Decor.SetActive(false);
                DefoltAudio.Play();
                FightAudio.Stop();
                Spaceship.Stop();
            }
            SystemEvent.DoStartStep();
            if (AttackCount % 10 == 0)
                BossDirectionMediate = Instantiate(BossDirection, widtharray[BigAsteroidDirectionl], Quaternion.identity, transform);
        }
    }
    private void EndGame()
    {
        endGame = true;
    }
    public void StartGame()
    {
        endGame = false;
        SystemEvent.DoAddHealth();
    }
    public void AudioMute()
    {
        DefoltAudio.mute = true;
        FightAudio.mute = true;
        Spaceship.mute = true;
    }
    public void AudioPlay()
    {
        DefoltAudio.mute = false;
        FightAudio.mute = false;
        Spaceship.mute = false;
    }
}
