using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage;
    public float timeToLive = 3f;
    public GameObject caster;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Kill");
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject gmobj = collision.gameObject;
        if (gmobj.tag == "Enemy" || gmobj.tag == "Player" && gmobj != caster) // 8 = Entities
        {
            gmobj.GetComponent<ClassBase>().takeDamage(damage);
            gmobj.GetComponent<ClassBase>().explosion();
            // Debug.Log("first");
            Destroy(gameObject);
        }
        // else if(GetComponent<Collider>().tag == "Attack")
        // {
        //     // Debug.Log("second");
        //     Destroy(this.gameObject);
        // }
        
    }

    public IEnumerator Kill ()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy (gameObject);
    }
}
