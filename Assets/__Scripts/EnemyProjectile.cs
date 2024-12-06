using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Renderer rend;
    private Rigidbody rigid;

    [Header("Dynamic")]
    public float speed = 20f; // Speed of the projectile
    public float lifetime = 5f; // Time before the projectile self-destructs

    // Layer to check against (optional, if needed)
    public int nonDestructableLayer;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();

        // Set the layer index for NonDestructableBuilding (if applicable)
        nonDestructableLayer = LayerMask.NameToLayer("NonDestructableBuilding");
    }

    void Start()
    {
        // Destroy the projectile after a set time to prevent clutter
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward at a constant speed
        rigid.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Optional: Check for specific layer collision
        if (collision.gameObject.layer == nonDestructableLayer)
        {
            Destroy(gameObject); // Destroy the projectile on collision
        }

        // Destroy the projectile on any collision (adjust as needed)
        Destroy(gameObject);
    }

    // Property to manage projectile velocity (optional)
    public Vector3 vel
    {
        get { return rigid.velocity; }
        set { rigid.velocity = value; }
    }
}
