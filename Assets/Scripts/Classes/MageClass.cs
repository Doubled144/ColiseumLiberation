using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClass : ClassBase
{
    public GameObject fireBallPrefab;
    public float fireBallSpeed;
    public GameObject magicBurst;
    public GameObject specialAbilityPrefab;

    public override void _normalAttack ()
    {
        base.normalAttackSound.Play();
        GameObject fireBall = (GameObject) Instantiate(fireBallPrefab, base.attackPoint.position, base.attackPoint.rotation);
        Rigidbody rb = fireBall.GetComponent<Rigidbody>();
        FireBall fireBallScript = fireBall.GetComponent<FireBall>();
        fireBallScript.caster = gameObject;
        fireBallScript.damage = base.normalAttackDamage;
        rb.velocity = base.attackPoint.up * fireBallSpeed;
    }

    public override void _heavyAttack ()
    {
        base.heavyAttackSound.Play();
        GameObject magicAttack = (GameObject) Instantiate(magicBurst, new Vector3(base.attackPoint.position.x,
                                                                                base.attackPoint.position.y,
                                                                                base.attackPoint.position.z), base.attackPoint.rotation);
        magicPop magicScript = magicAttack.GetComponent<magicPop>();
        magicScript.caster = gameObject;
        magicScript.damage = base.heavyAttackDamage;
        
    }

    public override void _specialAttack (Vector3 pos)
    {
        base.specialAttackSound.Play();
        if (pos != null)
        {
            GameObject AoE_Attack = (GameObject) Instantiate(specialAbilityPrefab, pos, specialAbilityPrefab.transform.rotation);
            AoEBase aoeScript = AoE_Attack.GetComponent<AoEBase>();
            aoeScript.caster = gameObject;
        }
    }
}
