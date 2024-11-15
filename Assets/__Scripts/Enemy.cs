using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float health = 1;

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
