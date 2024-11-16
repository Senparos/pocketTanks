using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float health = 1;
    public float speed = 3f; // Speed of movement
    public float changeDirectionTime = 3f; // Time before changing direction
    public float mapBoundary = 10f; // Boundaries for the map

    private Vector3 movementDirection;
    private float timer;

    private void Start()
    {
        // Initialize timer and movement direction
        timer = changeDirectionTime;
        ChooseRandomDirection();
    }

    private void Update()
    {
        Move();

        // Update the timer for changing direction
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = changeDirectionTime;
            ChooseRandomDirection();
        }
    }

    private void Move()
    {
        // Move the enemy in the chosen direction
        transform.position += movementDirection * speed * Time.deltaTime;

        // Keep the enemy within map boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -mapBoundary, mapBoundary),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -mapBoundary, mapBoundary)
        );
    }

    private void ChooseRandomDirection()
    {
        // Pick a random direction for the enemy to move
        movementDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void OnCollisionEnter(Collision coll)
    {
        // Get the GameObject that collided
        GameObject otherGO = coll.gameObject;

        // Check if it is a PlayerProjectile
        PlayerProjectile p = otherGO.GetComponent<PlayerProjectile>();
        if (p != null)
        {
            // Destroy the enemy and the projectile
            Destroy(this.gameObject);
            Destroy(otherGO);
        }
        else
        {
            // Log when colliding with other objects
            Debug.Log("Enemy hit by non-projectile object: " + otherGO.name);
        }
    }
}
