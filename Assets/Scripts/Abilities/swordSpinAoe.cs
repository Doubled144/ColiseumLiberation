using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordSpin : MonoBehaviour
{
    public string nameSearch;

    public GameObject caster; 
    public float rotationSpeed;
    private GameObject knight;
    // Start is called before the first frame update
    void Start()
    {
        knight = GameObject.Find(nameSearch).transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
        transform.position = new Vector3 (knight.transform.position.x, knight.transform.position.y +0.5f,knight.transform.position.z);
    }
}
