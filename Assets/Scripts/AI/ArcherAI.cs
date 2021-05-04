using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : AIbase
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
                if (base.canSpecialAtt)
                {
                    specialAttack ();
                    return;
                }

                if (base.canHeavyAtt)
                {
                    heavyAttack ();
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
            float arrowTime = base.distance/40f; //40 is projectile speed
            preditedPlayerLocation = base.opponentPos + base.opponentVelocity*arrowTime + base.opponentVelocity*base.decision_rate;
        }

        preditedPlayerLocation.y = transform.position.y;
        transform.LookAt (preditedPlayerLocation);
        Debug.DrawLine(transform.position, preditedPlayerLocation, Color.black, base.classScript.normalAttackRate);
        StartCoroutine (base.classScript.normalAttack());
    }

    private void heavyAttack ()
    {
        Vector3 preditedPlayerLocation = base.opponentPos; 

        if (Mathf.Abs(base.opponentMag) > 12)
        {
            float arrowTime = base.distance/40f;//base.classScript.arrowSpeed;
            preditedPlayerLocation = base.opponentPos + base.opponentVelocity*arrowTime + base.opponentVelocity*base.decision_rate;
        }

        preditedPlayerLocation.y = transform.position.y;
        transform.LookAt (preditedPlayerLocation);
        Debug.DrawLine(transform.position, preditedPlayerLocation, Color.black, base.classScript.normalAttackRate);
        StartCoroutine (base.classScript.heavyAttack());
    }

    private void specialAttack ()
    {
        Vector3 preditedPlayerLocation = base.opponentPos; 

        if (Mathf.Abs(base.opponentMag) > 12)
        {
            preditedPlayerLocation = base.opponentPos + base.opponentVelocity + base.opponentVelocity*base.decision_rate*0.5f;
        }

        preditedPlayerLocation.y = transform.position.y;
        transform.LookAt (preditedPlayerLocation);
        StartCoroutine (base.classScript.specialAttack(preditedPlayerLocation));
    }
}
