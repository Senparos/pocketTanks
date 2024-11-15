using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private Renderer rend;

    [Header("Dynamic")]
    public Rigidbody rigid;

    // Layer to check against
    public int nonDestructableLayer;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();

        // Set the layer index for NonDestructableBuilding
        nonDestructableLayer = LayerMask.NameToLayer("NonDestructableBuilding");
    }

    // Destroy the projectile when it collides with anything in the NonDestructableBuilding layer
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object's layer matches NonDestructableBuilding
        if (collision.gameObject.layer == nonDestructableLayer)
        {
            Destroy(gameObject); // Destroy the projectile
        }
    }

    // Property to manage projectile velocity
    public Vector3 vel
    {
        get { return rigid.velocity; }
        set { rigid.velocity = value; }
    }
}
