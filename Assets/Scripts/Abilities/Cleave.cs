using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleave : MonoBehaviour
{
    public float damage;
    public float timeToLive = 3f;
    public GameObject caster;
    public float dist = 10f;
    public float speed = 2f;

    private BoxCollider col;
    private Vector3 dest;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        dest = col.center;
        dest.x = dist;
    }

    void Update()
    {
        col.center = Vector3.MoveTowards(col.center, dest, speed*Time.deltaTime);
        if (col.center == dest)
        {
            Destroy (gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject gmobj = collision.gameObject;

        if (gmobj.layer == 12 || gmobj.layer == 9 && gmobj != caster) // 8 = Entities
        {
            gmobj.GetComponent<ClassBase>().takeDamage(damage);
        }
        
    }
}
