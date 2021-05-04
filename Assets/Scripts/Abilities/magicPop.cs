using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicPop : MonoBehaviour
{
    public float damage;
    public float timeToLive = .5f;
    public GameObject caster;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Kill");
    }
    void OnTriggerEnter(Collider collision)
    {
        GameObject gmobj = collision.gameObject;
     
        if (gmobj.layer == 12 || gmobj.layer == 9  && gmobj != caster)
        {
            ClassBase classScript = gmobj.GetComponent<ClassBase>();
            classScript.takeDamage(damage);
            
            // classScript.rb.AddForce(moveDirection.normalized * 500f);
            StartCoroutine(knockback(gmobj));
        }
        
    }

    public IEnumerator knockback (GameObject gmobj)
    {
        ClassBase classScript = gmobj.GetComponent<ClassBase>();
        Vector3 moveDirection = gmobj.transform.position - caster.transform.GetChild(0).gameObject.transform.position;
        classScript.rb.velocity = moveDirection*8f;
        yield return new WaitForSeconds(0.5f);
        classScript.rb.velocity = new Vector3(0,0,0);
    }
    public IEnumerator Kill ()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy (gameObject);
    }
}
