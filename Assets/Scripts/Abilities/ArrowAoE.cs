using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAoE : AoEBase
{
    public GameObject arrowPrefab;

    public float damage = 10;
    public float speed = 5;
    public int projectilesPerSecond = 4;

    private int arrowSpawnHeight = 10;
    void Start()
    {
        base.Start();
        StartCoroutine(arrowAnimation());
    }

    public void Update()
    {
        base.Update();
    }

    private IEnumerator arrowAnimation ()
    {
        while (true)
        {
            for (int i = 0; i < projectilesPerSecond; i++)
            {
                Vector3 spawnPos = transform.position;
                float radius = 3;
                // spawn arrows <arrowSpawnHeight> up in a random range within the collider
                spawnPos.x += Random.Range(-radius, radius);
                spawnPos.z += Random.Range(-radius, radius);
                spawnPos.y += arrowSpawnHeight;

                GameObject arrow = (GameObject) Instantiate(arrowPrefab, spawnPos, transform.rotation);
                float spray = 15;
                float randomRot = Random.Range(-spray, spray);
                float randomRot2 = Random.Range(-spray, spray);
                arrow.transform.Rotate(new Vector3(randomRot, 0, randomRot2));

                Rigidbody rb = arrow.GetComponent<Rigidbody>();
                Arrow arrScript = arrow.GetComponent<Arrow>();
                arrScript.damage = 0;
                arrScript.timeToLive = 2;
                rb.velocity = -arrow.transform.up * speed;
                yield return new WaitForSeconds(1f/projectilesPerSecond);
            }
        }
    }

    public override void _applyEffect (GameObject entity)
    {
        ClassBase classScript = entity.GetComponent<ClassBase>() as ClassBase;

        if (classScript)
        {
            classScript.takeDamage (damage);
        }
    }
}
