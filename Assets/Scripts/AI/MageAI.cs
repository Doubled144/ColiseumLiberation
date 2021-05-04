using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAI : AIbase
{
    private bool repositioning = false;
    private bool inRange = false;

    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        inRange = base.distance <= base.distToOpponent;
        //print ("InRange: " + inRange + ", repos: " + repositioning);

        if (!inRange || repositioning) 
        {
            //transform.LookAt(base.nextPoint);
            base.moving = true;
            bool res = moveAlongPath ();
            if (res) { 
                repositioning = false; 
                //print("reached repath");
            }
        }
        else
        {
            base.moving = false;
        }

        base.FixedUpdate();
    }

    public override void makeDecision()
    {
        // if were within range
        if (inRange) 
        { 
            if (!repositioning)
            {
                if (base.canHeavyAtt && base.distance < 4f)
                {
                    Vector3 playerLoc = base.opponentPos;
                    playerLoc.y = transform.position.y;
                    transform.LookAt(playerLoc);
                    StartCoroutine (base.classScript.heavyAttack());
                    return;
                }

                if (base.canSpecialAtt)
                {
                    specialAttack ();
                    return;
                }


                if (base.canNormalAtt)
                {
                    normalAttack ();
                    return;
                }

                if (base.lastMoved >= 1.5f)
                {
                    rePosition ();
                }
            }
        } 
        // out of range of opponent
        else {
            repositioning = false;
            rePath (base.opponentPos);
        }

    }

    private void rePosition ()
    {
        repositioning = true;
        Vector3 moveTo = base.nextPoint;
        float s = 6f;
        moveTo.x += Random.Range(-s, s);
        moveTo.z += Random.Range(-s, s);
        rePath (moveTo);
    }

    private void normalAttack ()
    {
        Vector3 preditedPlayerLocation = base.opponentPos; 

        if (Mathf.Abs(base.opponentMag) > 12)
        {
            float arrowTime = base.distance/20f;
            preditedPlayerLocation = base.opponentPos + base.opponentVelocity*arrowTime + base.opponentVelocity*base.decision_rate;
        }

        preditedPlayerLocation.y = transform.position.y;
        transform.LookAt (preditedPlayerLocation);
        Debug.DrawLine(transform.position, preditedPlayerLocation, Color.black, base.classScript.normalAttackRate);
        StartCoroutine (base.classScript.normalAttack());
    }

    private void specialAttack ()
    {
        Vector3 preditedPlayerLocation = base.opponentPos; 
        preditedPlayerLocation.y = transform.position.y;

        if (Mathf.Abs(base.opponentMag) > 20)
        {
            preditedPlayerLocation = base.opponentPos + base.opponentVelocity*base.decision_rate*0.3f;
        }

        transform.LookAt (preditedPlayerLocation);
        StartCoroutine (base.classScript.specialAttack(preditedPlayerLocation));
    }
}

