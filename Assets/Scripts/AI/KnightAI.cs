using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI : AIbase
{

    private bool repositioning = false;
    private GameObject knightSword;

    public override void Start()
    {
        base.Start();
        knightSword = GameObject.Find("Knight Warrior Enemy").transform.GetChild(0).gameObject.transform.GetChild(2).gameObject;
    }

    public override void FixedUpdate()
    {
        // update path to target
        if (!repositioning && Time.time - base.lastRepath >= base.repathRate)
        {
            rePath (base.opponentPos);
        }

        // Finished Running away
        if (base.runAway && base.distance >= 15f)
        {
            moving = false;
            animator.SetFloat("Velocity", 0f);
            animator.SetBool("Moving", false);
            return;
        }
        // Repositioning
        else if (repositioning)
        {
            bool res = moveAlongPath ();
            if (res) { 
                repositioning = false;
            }
        }
        // Moving to Opponent
        else if (base.runAway || base.distance >= base.distToOpponent) 
        {             
            Vector3 temp = base.opponentPos;
            temp.y = transform.position.y;
            base.moving = true;
            transform.LookAt(base.opponent.transform);
            moveAlongPath ();
        }
        // Reached destination
        else
        { 
            Vector3 temp = base.opponentPos;
            temp.y = transform.position.y;
            base.moving = false;
            transform.LookAt(base.opponent.transform);
        }

        if (base.canNormalAtt && base.distance < 4)
        {
            StartCoroutine (classScript.normalAttack());
        }

        base.FixedUpdate();
    }

    public override void makeDecision()
    {
        // Low on health and can't attack (run away)
        if (base.myHealth < 30 && !base.canHeavyAtt && !base.canSpecialAtt)
        {
            base.runAway = true;
        }
        // Low on health and can attack (run back)
        else if (base.myHealth < 30 && (base.canHeavyAtt || base.canSpecialAtt))
        {
            // Check if close to enemy
            if (path.Count < 4)
            {
                if (base.canSpecialAtt)
                {
                    StartCoroutine (classScript.specialAttack(base.opponentPos));
                }
                else if (base.canHeavyAtt)
                {
                    StartCoroutine (classScript.heavyAttack());
                }
            }
            
            base.runAway = false;
        }
        // Far away from opponent
        else if (base.distance >= base.distToOpponent) 
        { 
            base.runAway = false;
            if (base.canHeavyAtt && path.Count < 4)
            {
                StartCoroutine (classScript.heavyAttack());
            }
        }
        // Reached opponent with sword
        else
        {
            base.runAway = false;

            if (base.canSpecialAtt)
            {
                StartCoroutine (classScript.specialAttack(base.opponentPos));
            }
            else if (base.canHeavyAtt && knightSword.activeSelf)
            { 
                StartCoroutine (classScript.heavyAttack());
                rePosition();
            }
        }
        
    }

    private void rePosition ()
    {  
        Vector3 moveTo = base.nextPoint;
        float s = 6f;
        moveTo.x += Random.Range(-s, s);
        moveTo.z += Random.Range(-s, s);
        bool test = rePath (moveTo);
        if (!test)
        {
            rePosition();
        }
        repositioning = true;
    }
}
