using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordLaunch : MonoBehaviour
{
    public string nameSearch;
    public float rotationSpeed;

    public float damage;
    public float timeToLive = 3f;
    public GameObject caster;
    public float timer;
    public GameObject restoreSword;
    private GameObject knightSword;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Kill");
        knightSword = GameObject.Find(nameSearch).transform.GetChild(0).gameObject.transform.GetChild(2).gameObject;
        transform.Rotate(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Rotate(0f, rotationSpeed, 0f);
        if(timer >= 1f && rotationSpeed >= 1)
        {
            rotationSpeed -= 1;
            timer = 0;
        }
    }
    public IEnumerator Kill ()
    {
        yield return new WaitForSeconds(timeToLive);
        if(knightSword)
        {
            Instantiate(restoreSword, knightSword.transform.position, knightSword.transform.rotation);
            knightSword.SetActive(true);
        }
        Destroy (gameObject);
    }
    
    public void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.CompareTag("Enemy") && caster.gameObject.name != "Knight Warrior Enemy")
        {
            Instantiate(restoreSword, knightSword.transform.position, knightSword.transform.rotation);
            knightSword.SetActive(true);
            collider.gameObject.GetComponent<ClassBase>().takeDamage(damage);
            Destroy(this.gameObject);
        }
        else if(collider.gameObject.CompareTag("Player") && caster.gameObject.name != "Knight Warrior")
        {
            Instantiate(restoreSword, knightSword.transform.position, knightSword.transform.rotation);
            knightSword.SetActive(true);
            collider.gameObject.GetComponent<ClassBase>().takeDamage(damage);
            Destroy(this.gameObject);
        }
        else if(collider.gameObject.CompareTag("Attack"))
        {
            Instantiate(restoreSword, knightSword.transform.position, knightSword.transform.rotation);
            knightSword.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
