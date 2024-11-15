// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum eWeaponType
// {
//     none,
//     blaster,
//     spread,
//     phaser,
//     missle,
//     laser,
//     shield
// }

// [System.Serializable]
// public class WeaponDefinition
// {
//     public eWeaponType type = eWeaponType.none;
//     public string letter;
//     public Color powerUpColor = Color.white;
//     public GameObject weaponModelPrefab;
//     public GameObject projectilePrefab;
//     public Color projectileColor = Color.white;
//     public float damageOnHit = 0;
//     public float damagePerSec = 0;
//     public float delayBetweenShots = 0;
//     public float velocity = 50;
// }

// public class Weapon : MonoBehaviour
// {
//     static public Transform PROJECTILE_ANCHOR;

//     [Header("Dynamic")]
//     [SerializeField]
//     private eWeaponType _type = eWeaponType.none;
//     public WeaponDefinition def;
//     public float nextShotTime;

//     private GameObject weaponModel;
//     private Transform shotPointTrans;

//     // Projectile settings to match FireProjectile requirements
//     public GameObject projectilePrefab;
//     public Transform projectileSpawnPoint;
//     public float projectileSpeed = 50f;

//     void Start()
//     {
//         if (PROJECTILE_ANCHOR == null)
//         {
//             GameObject go = new GameObject("_ProjectileAnchor");
//             PROJECTILE_ANCHOR = go.transform;
//         }

//         shotPointTrans = transform.GetChild(0);
//         SetType(_type);

//         Hero hero = GetComponentInParent<Hero>();
//         if (hero != null) hero.fireEvent += Fire;
//     }

//     public eWeaponType type
//     {
//         get { return _type; }
//         set { SetType(value); }
//     }

//     public void SetType(eWeaponType wt)
//     {
//         _type = wt;
//         if (_type == eWeaponType.none)
//         {
//             gameObject.SetActive(false);
//             return;
//         }
//         else
//         {
//             gameObject.SetActive(true);
//         }

//         //def = Main.GET_WEAPON_DEFINITION(_type);

//         if (weaponModel != null) Destroy(weaponModel);
//         weaponModel = Instantiate(def.weaponModelPrefab, transform);
//         weaponModel.transform.localPosition = Vector3.zero;
//         weaponModel.transform.localScale = Vector3.one;

//         nextShotTime = 0;
//     }

//     private void Fire()
//     {
//         if (!gameObject.activeInHierarchy || Time.time < nextShotTime) return;

//         switch (_type)
//         {
//             case eWeaponType.blaster:
//                 FireProjectile();
//                 break;
//             case eWeaponType.spread:
//                 FireProjectile();  // Center shot
//                 FireProjectile(10); // 10 degrees right
//                 FireProjectile(-10); // 10 degrees left
//                 break;
//             // Additional cases for other weapon types can be added here
//         }

//         nextShotTime = Time.time + def.delayBetweenShots;
//     }

//     public void FireProjectile(float angleOffset = 0)
//     {
//         if (projectilePrefab != null && projectileSpawnPoint != null)
//         {
//             GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
//             Rigidbody rb = projectile.GetComponent<Rigidbody>();

//             if (rb != null)
//             {
//                 // Apply rotation offset for spread shots
//                 Quaternion rotationOffset = Quaternion.Euler(0, 0, angleOffset);
//                 rb.velocity = rotationOffset * projectileSpawnPoint.forward * projectileSpeed;
//             }

//             Destroy(projectile, 5f);  // Optionally destroy projectile after 5 seconds
//         }
//         else
//         {
//             Debug.LogWarning("ProjectilePrefab or ProjectileSpawnPoint is not assigned.");
//         }
//     }
// }
