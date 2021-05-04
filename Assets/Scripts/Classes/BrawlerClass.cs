using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerClass : ClassBase
{
    public GameObject cleavePrefab;
    public GameObject shieldPrefab;
    private bool inSight;

    public override void _normalAttack ()
    {
        if (base.normalAttackSound != null)
        {
            base.normalAttackSound.Play();
        }
        
        if(inSight)
        {
            base.normalAtk = true;
        }
    }

    public override void _heavyAttack ()
    {
        GameObject cleaveAttack = (GameObject) Instantiate(cleavePrefab, base.attackPoint.position, base.attackPoint.rotation);
        Cleave script = cleaveAttack.GetComponent<Cleave>();
        script.damage = base.heavyAttackDamage;
        script.caster = gameObject;
        base.heavyAttackSound.Play();
        if(inSight)
        {
            base.heavyAtk = true;
        }   
    }

    public override void _specialAttack (Vector3 pos)
    {
        GameObject shield = (GameObject) Instantiate(shieldPrefab, transform.position, transform.rotation);
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player") 
        {
            inSight = true;
            if(normalAtk)
            {
                collider.GetComponent<ClassBase>().takeDamage(normalAttackDamage);
                base.normalAtk = false;
            }
            else if(heavyAtk)
            {
                collider.GetComponent<ClassBase>().takeDamage(heavyAttackDamage);
                base.heavyAtk = false;
            }
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        inSight = false;
    }
}
