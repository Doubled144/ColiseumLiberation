using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projector;
    public float moveSpeed = 2f;
    [HideInInspector]
    public Vector3 movement;

    [HideInInspector]
    public Rigidbody rb;
    
    public LayerMask ground;

    // Privates
    private Camera cam;
    //private Vector3 mousePos;
    private Animator animator;
    private ClassBase classScript;
    private float oldDistance = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        animator = GetComponentInChildren<Animator>();
        classScript = GetComponentInChildren<ClassBase>();
    }

    void Update()
    {
        if(classScript.isGuarding == false)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = 0;
            movement.z = Input.GetAxis("Vertical");
        }
        var dist = Mathf.Abs(rb.position.z - Camera.main.transform.position.z);
        var v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        var distanceBetween = Vector3.Distance(v3Pos, transform.position);
        
        if (movement.x == 0 && movement.z == 0)
        {
            animator.SetFloat("Velocity", 0f);
            animator.SetBool("Moving", false);
        }
        else
        {
            // Keep the next line if we're going to make the camera move around
            // var dist = Vector3.Distance(Camera.main.transform.position, transform.position);
            if (oldDistance < distanceBetween)
            {
                animator.SetFloat("Animation Speed", -1.0f);
                animator.SetFloat("Velocity", 1.0f);
                animator.SetBool("Moving", true);  
            }
            else if (oldDistance > distanceBetween)
            {
                animator.SetFloat("Velocity", 1.0f);
                animator.SetBool("Moving", true);
                animator.SetFloat("Animation Speed", 1.0f);
            }
        }
        oldDistance = distanceBetween;

        playerInput ();
        showIndicator ();
    }

    void playerInput ()
    {
        if (Input.GetAxis("Fire1") > 0f && classScript.isGuarding == false)
        {
            StartCoroutine (classScript.normalAttack());
        }
        if (Input.GetAxis("Fire2") > 0f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            classScript.isGuarding = true;
            animator.SetFloat("Animation Speed", 0f);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1) == false)
        {
            classScript.isGuarding = false;
            animator.SetFloat("Animation Speed", 1f);
        }
        if (Input.GetKeyDown("e"))
        {
            RaycastHit hit; 
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); 

            if (Physics.Raycast (ray, out hit, 100.0f)) 
            {
                if (hit.transform.gameObject.layer == 8) //ground
                {
                    Vector3 mousePos = hit.point;
                    StartCoroutine (classScript.specialAttack(mousePos));
                }
            }
        }
        if (Input.GetKeyDown("f") && classScript.isGuarding == false)
        {
            StartCoroutine (classScript.heavyAttack());
        }
    }

    void FixedUpdate()
    {
        if (!classScript.frozen && !classScript.isGuarding)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        } 

        //Handles character always aiming towards mouse
        RaycastHit hit; 
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); 

        if (Physics.Raycast (ray, out hit, 100.0f)) {
            if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 12)
            {
                Vector3 lookHere = hit.point;
                lookHere.y = transform.position.y;
                transform.LookAt (lookHere);
            }
        }
    }


    public void showIndicator ()
    {
        RaycastHit hit; 
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
        LayerMask mask = LayerMask.GetMask("Ground");
        //LayerMask mask2 = LayerMask.GetMask("Projectile");

        // mask = mask | mask2;
        // mask = ~mask;

        if (Physics.Raycast (ray, out hit, 100.0f, mask)) 
        {
            Vector3 mousePos = hit.point;
            mousePos.y += 10;
            if (projector != null)
            {
                projector.SetActive (true);
                projector.gameObject.transform.position = mousePos;
            }
        }
    }
}
