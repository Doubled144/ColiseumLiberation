using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public float timeToLive = 3f;
    public GameObject caster;
    private bool stuck = false;
    private bool didDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Kill");
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject gmobj = collision.gameObject;
        if (!stuck && gmobj != caster && gmobj.layer != 10) // 10 is projectiles
        {
            StartCoroutine(stickAndDestroy(gmobj));
        }

        if ((gmobj.layer == 9 || gmobj.layer == 12) && gmobj != caster && !didDamage) // 9 = Entities
        {
            gmobj.GetComponent<ClassBase>().takeDamage(damage);
            //Destroy (gameObject);
        }

        
    }

    private IEnumerator stickAndDestroy (GameObject gmobj)
    {
        stuck = true;
        GetComponent<Collider>().enabled = false;   
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        GetComponent<Rigidbody>().isKinematic = true;
        
        if(gameObject.tag == "Normal Arrow")
        {
            gameObject.transform.Translate(0.5f * -Vector3.up); // move the arrow deep inside
            gameObject.transform.parent = gmobj.transform;
        }
        else
        {
            if(gmobj.tag == "Enemy" || gmobj.tag == "Player")
            {
                gameObject.transform.Translate(0.5f * -Vector3.up); // move the arrow deep inside
                gameObject.transform.parent = gmobj.transform;
            }
        }
       
        //RigidbodyConstraints wiggling = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        //GetComponent<Rigidbody>().constraints = wiggling;
        yield return new WaitForSeconds(2.5f);
        Destroy (gameObject);
    }

    public IEnumerator Kill ()
    {
        yield return new WaitForSeconds(timeToLive);
        if (!stuck)
        {
            Destroy (gameObject);
        }
    }
}
