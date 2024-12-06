using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float health = 1;
    public float speed = 3f; // Speed of movement
    public float changeDirectionTime = 3f; // Time before changing direction
    public float mapBoundary = 10f; // Boundaries for the map
    public float detectionRange = 15f; // Range within which the enemy can detect the player
    public float rotationSpeed = 5f; // Speed at which the enemy turns towards the player
    public GameObject projectilePrefab; // Projectile prefab
    public Transform firePoint; // Fire point for the projectile
    public float fireCooldown = 2f; // Cooldown between shots

    private Transform player;
    private Vector3 movementDirection;
    private float timer;
    private float lastFireTime;

    private void Start()
    {
        // Initialize timer and movement direction
        timer = changeDirectionTime;
        ChooseRandomDirection();

        // Find the player GameObject
        GameObject playerObject = GameObject.Find("Tank_9");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No object named 'Tank_9' found in the scene.");
        }
    }

    private void Update()
    {
        if (player != null && CanSeePlayer())
        {
            TurnTowardsPlayer();
            ShootAtPlayer();
        }
        else
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

    private bool CanSeePlayer()
    {
        // Check if the player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRange;
    }

    private void TurnTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Calculate the rotation step
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ShootAtPlayer()
    {
        // Shoot if cooldown has passed
        if (Time.time >= lastFireTime + fireCooldown)
        {
            lastFireTime = Time.time;

            if (projectilePrefab != null && firePoint != null)
            {
                // Instantiate and fire the projectile
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = firePoint.forward * 10f; // Adjust projectile speed as needed
                }
                Destroy(projectile, 5f); // Clean up the projectile after 5 seconds
            }
        }
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
