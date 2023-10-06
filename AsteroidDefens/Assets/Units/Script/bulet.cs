using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulet : MonoBehaviour
{
    private float speed = 4;
    public float damage;
    public GameObject target;
    private Vector3 targetPosition;
    public bool isMortira = false;
    private float explosionRadius = 0.7f;
    public GameObject ParticleSystem;

    private void Start()
    {
        Instantiate(ParticleSystem, transform.position, Quaternion.identity);
    }
    void Update()
    {
        if (target != null)
            targetPosition = target.transform.position;

        if (transform.position != targetPosition)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        else
            SetDestroy();
    }
    private void SetDestroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            if (isMortira)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.tag == "Asteroid")
                    {
                        collider.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
                    }
                }
            }
            else
            {
                collision.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
