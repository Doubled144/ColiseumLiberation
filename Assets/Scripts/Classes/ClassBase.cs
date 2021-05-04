using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassBase : MonoBehaviour
{
    //Stats
    public float health, defense;

    //Body components
    //public RuntimeAnimatorController player;

    //Attack system
    public Transform attackPoint;
    public float attackRange, normalAttackRate, heavyAttackRate, specialAttackRate;
    public float attackDamage, normalAttackDamage, heavyAttackDamage, specialAttackDamage;
    public bool normalAtk, heavyAtk;

    //Sound system
    public AudioSource normalAttackSound, heavyAttackSound, specialAttackSound, damageSound;
    [HideInInspector]
    public Rigidbody rb;

    //Private
    private Animator animator;
    public float lastNormalAttack, lastHeavyAttack, lastSpecialAttack = 0;
    
    //HealthBar objects
    public GameObject HealthBar;
    ProgressBar Health;

    //AttackBar Objects
    public GameObject specialBar;
    ProgressBarCircle special;
    public float specialEnergy, specialEnergyCost, specialIncrease;

    public GameObject normalBar;
    ProgressBarCircle normal;
    public float normalEnergy, normalEnergyCost, restoreNormal;

    public GameObject heavyBar;
    ProgressBarCircle heavy;
    public float heavyEnergy, heavyEnergyCost, restoreHeavy;

    public ParticleSystem explosionParticle;

    [HideInInspector]
    public bool frozen = false;
    public float normalAttackFreeze = 0;

    public bool isGuarding;

    private float normalTimer;
    private float heavyTimer;

    public GameObject spawnPoint;


//--------------------------------------------------------------------------------------------------//
// Start and Update                                                                                 //
//--------------------------------------------------------------------------------------------------//
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("Animation Speed", 1f);
        Health = HealthBar.GetComponent<ProgressBar>();
        if (specialBar)
            special = specialBar.GetComponent<ProgressBarCircle>();
        if(normalBar)
        {
            normal = normalBar.GetComponent<ProgressBarCircle>();
            normal.Alert = 100;
        }
            
        if(heavyBar)
        {
            heavy = heavyBar.GetComponent<ProgressBarCircle>();
            heavy.Alert = 100;
        }
            
        
        normalAttackFreeze = normalAttackRate;
        isGuarding = false;
        normalEnergy = 100f;
        heavyEnergy = 100f;
        
        
        _Start();
        StartCoroutine ("tick");
    }

    void Update()
    {
        Health.UpdateValue(health);
        if (specialBar)
            special.UpdateValue(specialEnergy);
        if(normalBar)
            normal.UpdateValue(normalEnergy);
        if(heavyBar)
            heavy.UpdateValue(heavyEnergy);
        normalTimer += Time.deltaTime;
        heavyTimer += Time.deltaTime;

        if(normalTimer >= 5f)
            restoreNormalEnergy();
        if(heavyTimer >= 10f)
            restoreHeavyEnergy();
        if(normalBar)
        {
            if (Time.time - lastNormalAttack > normalAttackRate && normalEnergy >= normalEnergyCost) 
                normal.Alert = 0;
        }
        if(heavyBar)
        {
            if (Time.time - lastHeavyAttack > heavyAttackRate && heavyEnergy >= heavyEnergyCost) 
                heavy.Alert = 0;
        }
        
    }
    public virtual void _Start()
    {
    
    }

//--------------------------------------------------------------------------------------------------//
// Helper Functions                                                                                 //
//--------------------------------------------------------------------------------------------------//

    public void reset ()
    {
        health = 100;
        normalAtk = false;
        heavyAtk = false;
        lastNormalAttack = 0;
        lastHeavyAttack = 0;
        lastSpecialAttack = 0;
        heavyEnergy = 100;
        normalEnergy = 100;
        isGuarding = false;
        gameObject.transform.position = spawnPoint.transform.position;
        rb.constraints = ~RigidbodyConstraints.FreezePosition;

        AIbase aiScript = GetComponent<AIbase>();
        if (aiScript)
        {
            StartCoroutine(aiScript.tick ());
        }
    }

    public IEnumerator normalAttack()
    {
        if (Time.time - lastNormalAttack > normalAttackRate && normalEnergy >= normalEnergyCost) 
        {
            lastNormalAttack = Time.time;
            normalEnergy -= normalEnergyCost;
            restoreSpecialEnergy();
            
            animator.SetInteger("Trigger Number", 2);
            animator.SetTrigger("Trigger");
            if (normalAttackRate != 0)
            {
                animator.SetFloat("Animation Speed", 1f / normalAttackRate);
                yield return new WaitForSeconds(0.4f);
                animator.SetFloat("Animation Speed", 1f);
            }
            if(normalBar)
                normal.Alert = 100;
            _normalAttack();
        }
    }

    public virtual void _normalAttack()
    {
       print ("Implement normal attack in subclass");
    }

    public IEnumerator heavyAttack()
    {
        if (Time.time - lastHeavyAttack > heavyAttackRate && heavyEnergy >= heavyEnergyCost) 
        {
            lastHeavyAttack = Time.time;
            heavyEnergy -= heavyEnergyCost;
            restoreSpecialEnergy();

            animator.SetInteger("Trigger Number", 2);
            animator.SetTrigger("Trigger");
            if (heavyAttackRate != 0)
            {
                animator.SetFloat("Animation Speed", 1f / heavyAttackRate );
                yield return new WaitForSeconds(0.4f);
                animator.SetFloat("Animation Speed", 1f);
            }
            if(heavyBar)
                heavy.Alert = 100;
            _heavyAttack();
        }
    }
    public virtual void _heavyAttack()
    {
       print ("Implement heavy attack in subclass");
    }


    public IEnumerator specialAttack(Vector3 pos)
    {
        if (Time.time - lastSpecialAttack > specialAttackRate && specialEnergy >= specialEnergyCost) 
        {
            specialEnergy -= specialEnergyCost;
            lastSpecialAttack = Time.time;
            animator.SetInteger("Trigger Number", 2);
            animator.SetTrigger("Trigger");
            if (specialAttackRate != 0)
            {
                animator.SetFloat("Animation Speed", 1f / specialAttackRate);
                yield return new WaitForSeconds(0.4f);
                animator.SetFloat("Animation Speed", 1f);
            }
            StartCoroutine (Freeze(normalAttackFreeze));
            _specialAttack(pos);
        }
    }

    public IEnumerator Freeze(float duration)
    {
        frozen = true;
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds (duration);
        frozen = false;
    }

    public virtual void _specialAttack(Vector3 pos)
    {
       print ("Implement special attack in subclass");
    }
   
    public virtual void takeDamage(float damage)
    {
        if(isGuarding)
           damage = Mathf.Abs(damage*(defense/100f));
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            this.gameObject.transform.position = new Vector3(0f, -10f, 0f);
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
        restoreSpecialEnergy();
    }
    public virtual void explosion()
    {
        Instantiate(explosionParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        
    }
    public virtual void restoreNormalEnergy()
    {
        if(normalEnergy <= 99f)
            normalEnergy += restoreNormal;
        normalTimer = 0;
    }
    public virtual void restoreHeavyEnergy()
    {
        if(heavyEnergy <= 99f)
            heavyEnergy += restoreHeavy;
        heavyTimer = 0;
    }
    public virtual void restoreSpecialEnergy()
    {
        if(specialEnergy < 100f)
            specialEnergy += specialIncrease;
    }

    public IEnumerator tick()
    {
        while(health > 0)
        {
            yield return new WaitForSeconds(1f);
            if(specialEnergy < 100)
                specialEnergy += 2f;
        }  
    }
}