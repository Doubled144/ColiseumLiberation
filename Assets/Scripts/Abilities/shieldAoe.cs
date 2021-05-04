using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldAoe : MonoBehaviour
{
    public string nameSearch;
    public string tag;

    public float damage;
    public float timeToLive = 10f;
    public GameObject caster;
    private float timer;
    private GameObject brawler;

    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound.Play();
        StartCoroutine("Kill");
        GameObject temp = GameObject.Find(nameSearch);
        if (temp != null)
        {
            brawler = temp.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject;
        }
        timer += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
       if(GameObject.Find(nameSearch).GetComponent<ClassBase>().health > 0)
        {
            GameObject user = GameObject.Find(nameSearch);
            transform.position = new Vector3 (user.transform.position.x, user.transform.position.y,user.transform.position.z);
        }
        else if(GameObject.Find(nameSearch).GetComponent<ClassBase>().health <= 0)
            Destroy(this.gameObject);
    }
    void OnTriggerStay(Collider collider)
    {
        if(collider.GetComponent<BrawlerClass>() && collider.CompareTag(tag))
        {
            Physics.IgnoreCollision(collider.GetComponent<Collider>(),this.GetComponent<Collider>());
            timer += Time.deltaTime;
            if(timer >= 2f)
            {
                if(collider.GetComponent<BrawlerClass>().health <= 99)
                {
                    if(collider.GetComponent<BrawlerClass>().health % 2 == 0)
                        collider.GetComponent<ClassBase>().health += 4;
                    else
                        collider.GetComponent<ClassBase>().health += 5;
                    timer = 0f;
                }
                
            }
            
        }
        else
        {
            this.GetComponent<Collider>().isTrigger = false;
        }
        if(collider.CompareTag("Attack"))
        {
            Destroy(collider.gameObject);
        }

    }
    public IEnumerator Kill ()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy (gameObject);
    }
}
