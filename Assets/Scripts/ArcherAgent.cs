// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class ArcherAgent : Agent 
// {
//     private ClassBase classScript;
//     private ClassBase opponentClassScript;
//     private Rigidbody rb;
//     public GameObject opponent, spawnPt, spawnPtOpponent;
//     private float opponentHealth, health;
//     private Vector3 currentEulerAngles;
//     public float reward = 0.0f;
//     public float moveSpeed = 4f;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         classScript = GetComponentInChildren<ClassBase>();
//         opponentClassScript = opponent.GetComponent<ClassBase>();
//         //player = GameObject.FindGameObjectsWithTag("Player")[0];
//         opponentHealth = opponentClassScript.health;
//         health = classScript.health;
//     }

//     void FixedUpdate ()
//     {
//         float opponentUpdatedHealth = opponentClassScript.health;
//         float myUpdatedHealth = classScript.health;

//         // set positive reward if opponent loses health
//         if (opponentUpdatedHealth < opponentHealth)
//         {
//             float healthLostRatio = (opponentHealth - opponentUpdatedHealth)/100;
//             AddReward (healthLostRatio);
//             reward += healthLostRatio;
//             opponentHealth = opponentUpdatedHealth;
//         }

//         // set negative reward if we lose health
//         if (myUpdatedHealth < health)
//         {
//             float healthLostRatio = (health - myUpdatedHealth)/100;
//             //print ("Player Hit");
//             AddReward (-healthLostRatio);
//             reward -= healthLostRatio;
//             health = myUpdatedHealth;
//         }

//         if (opponentHealth == 0)
//         {
//             SetReward (1f);
//             EndEpisode ();
//         }

//         if (health == 0)
//         {
//             SetReward (-1f);
//             EndEpisode ();
//         }

//         // add speed incentive
//         AddReward(-0.001f);
//         reward -= 0.001f;
//     }

//     public override void OnEpisodeBegin()
//     {
//         // called after an episode finishes (resets environment)
//         //opponentClassScript.health = 100f;
//         classScript.health = 100f;
//         opponentClassScript.health = 100f;

//         opponentHealth = opponentClassScript.health;
//         health = classScript.health;

//         reward = 0f;

//         Vector3 SpawnPoint = spawnPt.transform.position;
//         SpawnPoint.x += Random.Range(-8, 8);
//         SpawnPoint.z += Random.Range(-4, 4);

//         Vector3 SpawnPointOpponent = spawnPtOpponent.transform.position;
//         SpawnPointOpponent.x += Random.Range(-8, 8);
//         SpawnPointOpponent.z += Random.Range(-4, 4);

//         gameObject.transform.position = SpawnPoint;
//         opponent.transform.position = SpawnPointOpponent;
//     }


//     public override void CollectObservations(VectorSensor sensor)
//     {
//         // pass information to AI
//         sensor.AddObservation(transform.localPosition);
//         sensor.AddObservation(opponent.transform.localPosition);
//         sensor.AddObservation(opponentClassScript.health);
//         sensor.AddObservation(classScript.health);
//     }

//     public Vector3 movement;
//     public override void OnActionReceived(ActionBuffers actions)
//     {
//         float x_off = actions.ContinuousActions[0];
//         float z_off = actions.ContinuousActions[1];
//         float x_movement = actions.ContinuousActions[2];
//         float z_movement = actions.ContinuousActions[3];
//         int firing = actions.DiscreteActions[0];

//         movement.x = x_movement;
//         movement.y = 0;
//         movement.z = z_movement;

//         // move agent toif (!classScript.frozen)
//         if (!classScript.frozen)
//         {
//             rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
//         } 

//         // rotate character to hopefully face player
//         float variance = 10f;
//         Vector3 offset = new Vector3(x_off, 0f, z_off) * variance;
//         Vector3 shootHere = offset + opponent.transform.position;

//         transform.LookAt(shootHere); 

//         // normal attack
//         if (firing == 1)
//         {
//             StartCoroutine (classScript.normalAttack());
//         } 
//     }
// }
