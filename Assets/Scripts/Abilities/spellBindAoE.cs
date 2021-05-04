using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBindAoE : AoEBase
{
    public float ForceSpeed;
    public float damage = 10;

    void Start()
    {
        base.Start();
    }

    public override void _applyEffect (GameObject entity)
    {
        if (!entity.transform) { return; }
        
        Vector3 location = new Vector3(transform.position.x, entity.transform.position.y, transform.position.z);
        Vector3 dir = location - entity.transform.position;
        Rigidbody rb = entity.GetComponent<Rigidbody>() as Rigidbody;

        if (rb != null)
        {
            rb.velocity = dir.normalized * ForceSpeed;
        }

        // entity.transform.position = Vector3.MoveTowards(entity.transform.position, location, ForceSpeed);
    }
}
