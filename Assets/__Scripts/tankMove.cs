using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tankMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 20f;
    public float fireCooldown = 0.5f;

    private float lastFireTime;
    private GameObject lastTriggerGo = null;

    [Header("Map Boundaries")]
    public float xMin = -25f; // Set based on your map size
    public float xMax = 25f;
    public float zMin = -25f;
    public float zMax = 25f;

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        // Movement forward/backward
        float moveDirection = Input.GetAxis("Vertical"); // W/S or Up/Down
        transform.Translate(Vector3.forward * moveDirection * moveSpeed * Time.deltaTime);

        // Rotation left/right
        float turnDirection = Input.GetAxis("Horizontal"); // A/D or Left/Right
        transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
    }

    void HandleShooting()
    {
        // Check if the player presses the fire button (e.g., left mouse button or space key)
        if (Input.GetButtonDown("Fire1") && Time.time >= lastFireTime + fireCooldown)
        {
            FireProjectile();
            lastFireTime = Time.time;
        }
    }

    public void FireProjectile()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Add velocity to the projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = projectileSpawnPoint.forward * projectileSpeed;
            }

            // Optionally, destroy the projectile after a set time to prevent clutter
            Destroy(projectile, 5f);
        }
        else
        {
            Debug.LogWarning("ProjectilePrefab or ProjectileSpawnPoint is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an EnemyProjectile
        EnemyProjectile enemyProjectile = other.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            Debug.Log("Tank has been hit by an enemy projectile!");
            
            // Destroy the projectile
            Destroy(other.gameObject);

            // Load the EndScreen scene
            SceneManager.LoadScene("EndScreen");
            return;
        }

        // Handle other collisions (e.g., power-ups)
        PowerUp pUp = other.GetComponent<PowerUp>();
        if (pUp != null)
        {
            AbsorbPowerUp(pUp);
        }
    }


    void ClampPositionToBounds()
    {
        // Get the current position
        Vector3 currentPosition = transform.position;

        // Clamp the position within the boundaries
        currentPosition.x = Mathf.Clamp(currentPosition.x, xMin, xMax);
        currentPosition.z = Mathf.Clamp(currentPosition.z, zMin, zMax);

        // Update the position
        transform.position = currentPosition;
    }

    public void AbsorbPowerUp(PowerUp pUp)
    {
        Debug.Log("Absorbed PowerUp");
        pUp.AbsorbedBy(this.gameObject);
    }
}
