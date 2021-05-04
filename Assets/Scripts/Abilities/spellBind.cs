using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBind : MonoBehaviour
{
    public GameObject pullObject;
    public float ForceSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        pullObject = other.gameObject;

        pullObject.transform.position = Vector3.MoveTowards(pullObject.transform.position, 
                                                            new Vector3(transform.position.x, pullObject.transform.position.y, transform.position.z), 
                                                            ForceSpeed * Time.deltaTime);
    }
}
