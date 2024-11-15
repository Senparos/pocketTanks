using System.Collections;
using System.Collections.Generic;
using TMPro; // Make sure to include the TextMeshPro namespace
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class PowerUp : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("x holds a min value and y a max value for a random range call")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    [Tooltip("x holds a min value and y a max value as above")]
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 10;
    public float fadeTime = 4;

    [Header("Dynamic")]
    //public eWeaponType _type; // Changed from type in the book
    public GameObject cube;
    public TextMeshPro letter; // Changed to TextMeshPro
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Material cubeMat;

    void Awake()
    {
        cube = transform.GetChild(0).gameObject;

        letter = GetComponentInChildren<TextMeshPro>(); // Use GetComponentInChildren to find the TextMeshPro component
        rigid = GetComponent<Rigidbody>();
        //bndCheck = GetComponent<BoundsCheck>();
        cubeMat = cube.GetComponent<Renderer>().material; // Get the material from the cube

        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = cubeMat.color;
            c.a = 1f - u;
            cubeMat.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

    }

    //public eWeaponType type { get { return _type; } set { SetType(value); } }

    // public void SetType(eWeaponType wt)
    // {
    //     WeaponDefinition def = Main.GET_WEAPON_DEFINITION(wt);
    //     cubeMat.color = def.powerUpColor;
    //     letter.text = def.letter; // Assuming def.letter is a string
    //     _type = wt; // Changed from _type
    // }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
