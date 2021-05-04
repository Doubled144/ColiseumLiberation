using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherClass : ClassBase
{
    public GameObject arrowPrefab;
    public GameObject heavyAbilityPrefab;
    public GameObject specialAbilityPrefab;
    public float arrowSpeed;

    public override void _normalAttack ()
    {
        GameObject arrow = (GameObject) Instantiate(arrowPrefab, base.attackPoint.position, base.attackPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Arrow arrScript = arrow.GetComponent<Arrow>();
        arrScript.caster = gameObject;
        arrScript.damage = base.attackDamage;
        rb.velocity = -base.attackPoint.up * arrowSpeed;
        
    }
    public override void _heavyAttack ()
    {
        GameObject arrow = (GameObject) Instantiate(heavyAbilityPrefab, base.attackPoint.position, base.attackPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Arrow arrScript = arrow.GetComponent<Arrow>();
        arrScript.caster = gameObject;
        arrScript.damage = base.heavyAttackDamage;
        rb.velocity = -base.attackPoint.up * arrowSpeed;
    }

    public override void _specialAttack (Vector3 pos)
    {
        if (pos != null)
        {
            GameObject AoE_Attack = (GameObject) Instantiate(specialAbilityPrefab, pos, specialAbilityPrefab.transform.rotation);
            AoEBase aoeScript = AoE_Attack.GetComponent<AoEBase>();
            aoeScript.caster = gameObject;
        }
    }

}
