using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIbase : MonoBehaviour
{
    //          public members
    public float decision_rate = 0.5f;
    public float repathRate, distToNode, distToOpponent, moveSpeed;
    
    [HideInInspector]
    public GameObject opponent;
    [HideInInspector]
    public ClassBase classScript, opponentClassScript;
    [HideInInspector]
    public aStar pathfinding;
    [HideInInspector]
    public List<Vector3> path;
    [HideInInspector]
    public Vector3 nextPoint;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Vector3 movement;
    [HideInInspector]
    public float lastRepath = 0;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public bool moving = false;
    [HideInInspector]
    public bool runAway = false;

    //          decision making information
    public float lastSample = 0;
    [HideInInspector]
    private Vector3 myPrevLoc, opponentPrevLoc;
    [HideInInspector]
    public float myHealth, opponentHealth, distance, myMag, opponentMag;
    [HideInInspector]
    public Vector3 myPos, myVelocity, opponentPos, opponentVelocity;   
    [HideInInspector]
    public bool lineOfSight = false, canNormalAtt = true, canHeavyAtt = true, canSpecialAtt = true;

    [HideInInspector]
    public float lastMoved;

    
    public virtual void Start()
    {
        classScript = GetComponent<ClassBase>();
        rb = GetComponent<Rigidbody>();
        opponent = GameObject.FindGameObjectsWithTag("Player")[0];
        opponentClassScript = opponent.GetComponent<ClassBase>();
        pathfinding = GameObject.FindGameObjectsWithTag("A*")[0].GetComponent<aStar>();
        path = new List<Vector3> ();
        myPrevLoc = transform.position;
        opponentPrevLoc = opponent.transform.position;
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("Animation Speed", 1f);
        lastMoved = Time.time;

        _collectObservations ();
        // tick is called once every <decision_rate> seconds
        StartCoroutine(tick ());
    }

    public float lastMovedTime = 0;
    public virtual void FixedUpdate()
    {
        if (opponent == null)
        {
            return;
        }
        // update animation if moving or not
        if (moving)
        {
            animator.SetFloat("Velocity", 1.0f);
            animator.SetBool("Moving", true);
            animator.SetFloat("Animation Speed", 1.0f);
            lastMoved = 0;
            lastMovedTime = Time.time;
        }
        else
        {
            lastMoved = Time.time - lastMovedTime;
            animator.SetFloat("Velocity", 0f);
            animator.SetBool("Moving", false);
        }
    }

    public bool moveAlongPath ()
    {
        if (classScript.health <= 0){
            return false;
        }

        if (nextPoint != null && path != null && nextPoint != new Vector3(0, 0, 0))
        {
            // if player is within range
            float distToGoal = Vector3.Distance(nextPoint, transform.position);
            
            if (distToGoal <= distToNode)
            {
                if (path == null || path.Count < 1)
                {
                    moving = false;
                    return true;
                }
                nextPoint = path[0]; 
                nextPoint.y = transform.position.y;
                path.RemoveAt(0);
            }

            // move AI to goal
            Vector3 dirToPoint = (nextPoint - transform.position).normalized;
            if (runAway)
            {
                rb.MovePosition(rb.position - dirToPoint * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                
                Vector3 temp = rb.position + dirToPoint * moveSpeed * Time.fixedDeltaTime;
                temp.y = 0.7f;
                rb.MovePosition(temp);
            }
            
        }
        return false;
    }

    public bool rePath (Vector3 target)
    {
        lastRepath = Time.time;
        path = pathfinding.getPath (transform.position, target);
        if (path == null || path.Count < 1) 
        { 
            return false; 
        }
        path = pathfinding.gridListToWorldSpace (path);
        nextPoint = path[0];
        nextPoint.y = transform.position.y;

        return true;
    }


    // tick is called once every <decision_rate> seconds
    //      collects observations and makes decision
    public IEnumerator tick ()
    {
        // while alive
        while (classScript.health > 0 && opponent != null)
        {
            _collectObservations ();
            makeDecision ();
            yield return new WaitForSeconds(decision_rate);
        }
    }

    virtual public void makeDecision ()
    {
        return;
    }

    // collects all information relevant to the AI's decision making
    private void _collectObservations ()
    {
        myPos = transform.position;
        myPos.y += 1.5f;
        opponentPos = opponent.transform.position;
        opponentPos.y += 1.5f;
        distance = Vector3.Distance(myPos, opponentPos);

        myHealth = classScript.health;
        opponentHealth = opponentClassScript.health;
        if (lastSample != 0)
        {
            Vector3 opponentDistance = opponentPos - opponentPrevLoc;
            float timePassed = Time.time - lastSample;
            opponentVelocity = opponentDistance / timePassed;
            opponentMag = opponentDistance.sqrMagnitude;
            opponentPrevLoc = opponentPos;

            Vector3 myDistance = myPos - myPrevLoc;
            myVelocity = myDistance / timePassed;
            myMag = myDistance.sqrMagnitude;
            myPrevLoc = myPos;
        } 
        lastSample = Time.time;

        RaycastHit hitInfo;
        Vector3 adjOpponentPos = opponentPos;
        adjOpponentPos.y += 1.5f;
        Vector3 adjMyPos = myPos;
        adjMyPos.y += 1.5f;
        bool hit = Physics.Linecast (adjMyPos, adjOpponentPos, out hitInfo);
        if(hit == true && hitInfo.transform.gameObject.tag == "Player")
        {
            lineOfSight = true;
        }else{
            lineOfSight = false;
        }
        
        canNormalAtt = (Time.time - classScript.lastNormalAttack > classScript.normalAttackRate && classScript.normalEnergy >= classScript.normalEnergyCost);

        canHeavyAtt = (Time.time - classScript.lastHeavyAttack > classScript.heavyAttackRate && classScript.heavyEnergy >= classScript.heavyEnergyCost);

        canSpecialAtt = (Time.time - classScript.lastSpecialAttack > classScript.specialAttackRate && classScript.specialEnergy >= classScript.specialEnergyCost);
       
        //Debug.Log("AI health: " + myHealth);
        //Debug.Log("Player health: " + opponentHealth);
        //Debug.Log("AI velocity: " + myVelocity);
        //Debug.Log("Player velocity: " + opponentVelocity);
        //Debug.Log("AI Magnitude: " + myMag);
        //Debug.Log("Player Magnitude: " + opponentMag);
        //Debug.Log("lineOfSight: " + lineOfSight);
        //if (hit == true)
        //{
        //    Debug.Log("Hit: " + hitInfo.transform);
        //}
        //Debug.Log("Normal Attack: " + canNormalAtt);
        //Debug.Log("Heavy Attack: " + canHeavyAtt);
        //Debug.Log("Special Attack: " + canSpecialAtt);
    }
}
