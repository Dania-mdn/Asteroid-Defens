using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float rotationSpeed; // Скорость вращения объекта
    public GameObject model; int x;

    public float Defoltspeed;
    public float speed;
    public float Helth;
    public bool Boss = false;

    public Sprite[] sprite;
    public SpriteRenderer SpriteRenderer;
    public AsteroidController asteroidController;
    private GameObject MediateAsteroid;
    public GameObject explosion;

    private void Start()
    {
        SpriteRenderer.sprite = sprite[Random.Range(0, sprite.Length)];

        speed = Defoltspeed;

        x = Random.Range(-1, 1);
        if (x == 0)
            x = 1;
        rotationSpeed = Random.Range(15, 35);

        StartCoroutine(RotateContinuously());

        if (Boss)
        {
            SystemEvent.DoSpawnBoss(Helth);
        }
    }
    private void OnEnable()
    {
        SystemEvent.EndGame += EndGame;
    }
    private void OnDisable()
    {
        SystemEvent.EndGame -= EndGame;
    }
    private IEnumerator RotateContinuously()
    {
        while (true) // Зацикливаем выполнение корутины
        {
            model.transform.Rotate(Vector3.forward * x, rotationSpeed * Time.deltaTime);

            yield return null; // Ждем один кадр
        }
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -1), speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!Boss)
                SystemEvent.DoHitPlayer(gameObject);
            else
                SystemEvent.DoEndGame();
            SetDestrroy();
        }
    }
    public void TakeDamage(float Damage)
    {
        if (Helth > Damage)
        {
            Helth = Helth - Damage; 
            if (Boss)
            {
                SystemEvent.DoHitBoss(Helth);
            }
        }
        else
        {
            SystemEvent.DoDestroyAsteroid(gameObject);
            if (Boss)
            {
                GenerateAsteroid();
                SystemEvent.DoDestroyBoss();
            }
            SetDestrroy();
        }
    }
    private void GenerateAsteroid()
    {
        for (int i = 0; i < asteroidController.asteroidCount / 3; i++)
        {
            float x = Random.Range(-0.9f, 0.9f);
            float y = Random.Range(-0.9f, 0.9f);
            MediateAsteroid = Instantiate(asteroidController.Asteroid, transform.position + new Vector3(x, y, transform.position.z), Quaternion.identity, asteroidController.transform);
            asteroidController.Asteroids.Add(MediateAsteroid);
        }
    }
    private void EndGame()
    {
        SystemEvent.DoDestroyAsteroid(gameObject);
        SetDestrroy();
    }
    private void SetDestrroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }
}
