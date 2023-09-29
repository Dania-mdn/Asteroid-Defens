using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    private int width = 6;
    private float size = 0.8f;
    private Vector2[] widtharray;

    public float coldawn = 0.2f;
    private float timer;
    private int asteroidCount = 4;
    private int UpdateAsteroidCount = 3;
    private int SpawnAsteroidCount;
    private bool IsSpawn = false;
    private bool endGame = false;
    public int AttackCount;

    public GameObject BigAsteroid;
    private int BigAsteroidDirectionl;
    public GameObject Asteroid;
    public List<GameObject> Asteroids;

    private void OnEnable()
    {
        SystemEvent.EndStep += SetIsSpawn;
        SystemEvent.HitPlayer += SetList;
        SystemEvent.EndGame += EndGame;
    }
    private void OnDisable()
    {
        SystemEvent.EndStep -= SetIsSpawn;
        SystemEvent.HitPlayer -= SetList;
        SystemEvent.EndGame -= EndGame;
    }
    private void Start()
    {
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

        if(AttackCount != 0 && AttackCount % 10 == 0)
        {
            Instantiate(BigAsteroid, widtharray[BigAsteroidDirectionl], Quaternion.identity, transform);
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
    }
    private void SetList(GameObject Asteroid)
    {
        Asteroids.Remove(Asteroid);

        if (Asteroids.Count == 0 && !endGame)
            SystemEvent.DoStartStep();
    }
    private void EndGame()
    {
        endGame = true;
    }
}
