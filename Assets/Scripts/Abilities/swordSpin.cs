using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordSpinAoe : MonoBehaviour
{
    public string nameSearch;
    public GameObject caster; 
    public float rotationSpeed;
    private GameObject knight;
    public float damage;
    public float timeToLive = 10f;
    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Kill");
        sound.Play();
        knight = GameObject.Find(nameSearch).transform.GetChild(0).gameObject;
        damage = caster.GetComponent<ClassBase>().specialAttackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
        if(knight)
            transform.position = new Vector3 (knight.transform.position.x, knight.transform.position.y +0.5f,knight.transform.position.z);
        else
            Destroy (gameObject);
    }
    public IEnumerator Kill ()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy (gameObject);
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player") 
        {
            collider.GetComponent<ClassBase>().takeDamage(damage);
        }
    }
}
