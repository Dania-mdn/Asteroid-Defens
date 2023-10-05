using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] Asteroid;
    public List<GameObject> Asteroids;
    public GameObject BossDirection;
    private GameObject BossDirectionMediate;
    public ParticleSystem ParticleSystem;
    public Animation Animation;
    public GameObject plane;

    private void OnEnable()
    {
        SystemEvent.EndStep += SetIsSpawn;
        SystemEvent.DestroyAsteroid += SetList;
        SystemEvent.HitPlayer += SetList;
        SystemEvent.EndGame += EndGame;
    }
    private void OnDisable()
    {
        SystemEvent.EndStep -= SetIsSpawn;
        SystemEvent.DestroyAsteroid -= SetList;
        SystemEvent.HitPlayer -= SetList;
        SystemEvent.EndGame -= EndGame;
    }
    private void Start()
    {
        ParticleSystem.Stop();

        float posX = -size * width / 2f - size / 2f;

        widtharray = new Vector2[width];

        for(int i = 0; i < widtharray.Length; i++)
        {
            posX += size;
            widtharray[i] = new Vector2(posX, transform.position.y);
        }

        timer = coldawn;
    }
    private void Update()
    {
        if (!IsSpawn) return;

        if (AttackCount % 10 == 0)
        {
            GameObject MediateAsteroid;
            MediateAsteroid = Instantiate(BigAsteroid, widtharray[BigAsteroidDirectionl], Quaternion.identity, transform);
            MediateAsteroid.GetComponent<Asteroid>().asteroidController = this;
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
    private void GeneretedAsteroid()
    {
        int j = Random.Range(0, widtharray.Length); 
        int i = Random.Range(0, Asteroid.Length); 
        GameObject MediateAsteroid;
        MediateAsteroid = Instantiate(Asteroid[i], widtharray[j], Quaternion.identity, transform);
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

            if(!ParticleSystem.isPlaying)
            {
                ParticleSystem.Play();
                Animation.Play();
                plane.SetActive(false);
            }
        }
    }
    private void SetList(GameObject Asteroid)
    {
        Asteroids.Remove(Asteroid);

        if (Asteroids.Count == 0 && !endGame)
        {
            if (ParticleSystem.isPlaying)
            {
                ParticleSystem.Stop();
                Animation.Stop();
                plane.SetActive(true);
            }
            SystemEvent.DoStartStep();
            if(AttackCount % 10 == 0)
                BossDirectionMediate = Instantiate(BossDirection, widtharray[BigAsteroidDirectionl], Quaternion.identity, transform);
        }
    }
    private void EndGame()
    {
        endGame = true;
    }
}
