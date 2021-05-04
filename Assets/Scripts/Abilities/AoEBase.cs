using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEBase : MonoBehaviour
{
    SphereCollider col;
    public List<GameObject> entities = new List<GameObject>();
    public float duration, pulses;
    [HideInInspector]
    public float lastPulse = 0;
    // Should be set by calling class
    public GameObject caster;

    private int entitiesLayer = 9;

    // Start is called before the first frame update
    public void Start()
    {
        col = GetComponent<SphereCollider>() as SphereCollider;
        StartCoroutine("Kill");
        lastPulse = 0;
    }

    public void Update ()
    {
        showIndicator ();
        if (entities.Count > 0)
        {
            applyEffect ();
        }
    }

    public void applyEffect ()
    {
        // 10/5
        if (Time.time - lastPulse > (duration/pulses)) 
        {
            lastPulse = Time.time;
            for (int i = 0; i < entities.Count; i++)
            {
                GameObject entity = entities[i];
                if (entity == null)
                {
                    entities.RemoveAt(i);
                    continue;
                }
                _applyEffect (entity);
            }
        }
    }

    public virtual void _applyEffect(GameObject entity)
    {
        print ("Implement _applyEffect() in subclass");
    }

    void OnTriggerEnter(Collider c)
    {
        GameObject gmobj = c.gameObject;
        if (gmobj.tag == "Enemy" || gmobj.tag == "Player")
        {
            if (c.gameObject != caster)
            {
                entities.Add (c.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        GameObject gmobj = c.gameObject;
        if (gmobj.tag == "Enemy" || gmobj.tag == "Player")
        {
            if (c.gameObject != caster)
            {
                entities.Remove (c.gameObject);
            }
        }
    }

    void showIndicator ()
    {
        
    }

    // Removes gameObject after <duration> seconds
    public IEnumerator Kill ()
    {
        yield return new WaitForSeconds(duration);
        Destroy (gameObject);
    }
}
