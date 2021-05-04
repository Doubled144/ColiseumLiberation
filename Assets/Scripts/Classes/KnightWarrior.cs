using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightWarrior : ClassBase
{
    public string nameSearch;

    public GameObject bigSword;
    public GameObject smallSword;
    public float smallSwordSpeed;
    public float noWeaponAttack;
    private GameObject knightSword;
    private bool inSight;

    public override void _Start ()
    {
       knightSword = GameObject.Find(nameSearch).transform.GetChild(0).gameObject.transform.GetChild(2).gameObject;
    }

    public override void _normalAttack ()
    { 
        base.normalAttackSound.Play();
        if(inSight)
        {
            base.normalAtk = true;
        }
    }

    public override void _heavyAttack ()
    {
        if(knightSword.activeSelf)
        {
            if (inSight)
            {
                base.heavyAtk = true;
            }
            base.heavyAttackSound.Play();
            GameObject swordSmall;
            if(!GameObject.Find("swordSmall(Clone)"))
            {
                knightSword.SetActive(false);
                swordSmall = (GameObject) Instantiate(smallSword, base.attackPoint.position, base.attackPoint.rotation);
                Rigidbody rb = swordSmall.GetComponent<Rigidbody>();
                swordLaunch swordLaunchScript = smallSword.GetComponent<swordLaunch>();
                swordLaunchScript.caster = gameObject;
                swordLaunchScript.damage = base.heavyAttackDamage;
                rb.velocity = base.attackPoint.up * smallSwordSpeed;
            }
        }
   }
    
    public override void _specialAttack (Vector3 pos)
    {
        base.specialAttackSound.Play();
        GameObject swordBig = (GameObject) Instantiate(bigSword, transform.position, transform.rotation);
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player") 
        {
            
            inSight = true;
            if(normalAtk)
            {
                if(!knightSword.activeSelf)
                {
                    collider.GetComponent<ClassBase>().takeDamage(noWeaponAttack);
                }  
                else
                {
                    collider.GetComponent<ClassBase>().takeDamage(normalAttackDamage);
                }
                base.normalAtk = false;
            }
            else if(heavyAtk)
            {
                base.heavyAtk = false;
            }
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        inSight = false;
    }
}
