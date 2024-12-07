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
    private Vector3 targetDirection; // Target direction for smooth interpolation
    private float timer;
    private float lastFireTime;

    private void Start()
    {
        timer = changeDirectionTime;
        ChooseRandomDirection();

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
        if (player != null && CanSeeTank())
        {
            TurnTowardsTank();
            ShootAtTank();
        }
        Move();
        HandleRandomMovement();
    }

    private void Move()
    {
        // Gradually interpolate towards the target direction
        movementDirection = Vector3.Lerp(movementDirection, targetDirection, Time.deltaTime * 2f).normalized;

        // Move the enemy
        transform.position += movementDirection * speed * Time.deltaTime;

        // Adjust direction if hitting boundaries
        CheckBoundaries();
    }

    private void HandleRandomMovement()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = changeDirectionTime;
            StartCoroutine(PauseBeforeDirectionChange());
        }
    }

    private IEnumerator PauseBeforeDirectionChange()
    {
        // Optionally pause before changing direction
        yield return new WaitForSeconds(0.5f);
        ChooseRandomDirection();
    }

    private void ChooseRandomDirection()
    {
        targetDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void CheckBoundaries()
    {
        Vector3 pos = transform.position;

        // If the enemy hits a boundary, reflect the movement direction
        if (pos.x <= -mapBoundary || pos.x >= mapBoundary)
        {
            targetDirection = new Vector3(-targetDirection.x, 0, targetDirection.z);
        }
        if (pos.z <= -mapBoundary || pos.z >= mapBoundary)
        {
            targetDirection = new Vector3(targetDirection.x, 0, -targetDirection.z);
        }

        // Clamp position to stay within boundaries
        transform.position = new Vector3(
            Mathf.Clamp(pos.x, -mapBoundary, mapBoundary),
            pos.y,
            Mathf.Clamp(pos.z, -mapBoundary, mapBoundary)
        );
    }

    private bool CanSeeTank()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRange;
    }

    private void TurnTowardsTank()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ShootAtTank()
    {
        if (Time.time >= lastFireTime + fireCooldown)
        {
            lastFireTime = Time.time;

            if (projectilePrefab != null && firePoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = firePoint.forward * 10f;
                }
                Destroy(projectile, 5f);
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        PlayerProjectile p = otherGO.GetComponent<PlayerProjectile>();
        if (p != null)
        {
            Destroy(this.gameObject);
            Destroy(otherGO);
            FindObjectOfType<score>().AddScore(10);
        }
        else
        {
            Debug.Log("Enemy hit by non-projectile object: " + otherGO.name);
        }
    }
}
