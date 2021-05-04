using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameObject playerClass;
    private ClassBase playerClassScript;
    public static GameObject enemyClass;
    private ClassBase enemyClassScript;
    public static GameObject enemyTwoClass;
    private ClassBase enemyTwoClassScript;
    public static GameObject enemyThreeClass;
    private ClassBase enemyThreeClassScript;
    public static bool noEnemyClass;
    public static bool noEnemyTwoClass;
    public static bool noEnemyThreeClass;

    GameObject clonedPlayerObject;
    GameObject clonedEnemyObject;
    GameObject clonedEnemyTwoObject;
    GameObject clonedEnemyThreeObject;
    
    public GameObject canvas;
    public Text GameUIText;
    public Text PlayButtonText;
    public static bool gameOver;

    public GameObject aiSpawn1, aiSpawn2, aiSpawn3, playerSpawn;
    public List<GameObject> enemies;
    public GameObject player;
    
    // round managment
    public int curRound;
    public int aiPoints, playerPoints;
    public Text aiPointsText, playerPointsText, curRoundText;


    public void Start ()
    {
        enemies = new List<GameObject>();
        gameOver = false;
        curRound = 1;
        playerPoints = 0;
        string tempName = playerClass.name;
        clonedPlayerObject = Instantiate (playerClass, playerSpawn.transform.position, Quaternion.identity);
        clonedPlayerObject.name = tempName;
        playerClassScript = clonedPlayerObject.GetComponent<ClassBase>();
        playerClassScript.spawnPoint = playerSpawn;
        player = clonedPlayerObject;

        if (!noEnemyClass)
        {
            tempName = enemyClass.name;
            clonedEnemyObject = Instantiate (enemyClass, aiSpawn1.transform.position, Quaternion.identity);
            clonedEnemyObject.name = tempName;
            enemyClassScript = clonedEnemyObject.GetComponent<ClassBase>();
            enemyClassScript.spawnPoint = aiSpawn1;
            enemies.Add(clonedEnemyObject);
        }
        
        if (!noEnemyTwoClass)
        {
            tempName = enemyTwoClass.name;
            clonedEnemyTwoObject = Instantiate (enemyTwoClass, aiSpawn2.transform.position, Quaternion.identity);
            clonedEnemyTwoObject.name = tempName;
            enemyTwoClassScript = clonedEnemyTwoObject.GetComponent<ClassBase>();
            enemyTwoClassScript.spawnPoint = aiSpawn2;
            enemies.Add(clonedEnemyTwoObject);
        }
       
        if (!noEnemyThreeClass)
        {
            tempName = enemyThreeClass.name;
            clonedEnemyThreeObject = Instantiate (enemyThreeClass, aiSpawn3.transform.position, Quaternion.identity);
            clonedEnemyThreeObject.name = tempName;
            enemyThreeClassScript = clonedEnemyThreeObject.GetComponent<ClassBase>();
            enemyThreeClassScript.spawnPoint = aiSpawn3;
            enemies.Add(clonedEnemyThreeObject);
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            canvas.transform.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        roundManager ();

        curRoundText.text = ""+curRound;
        aiPointsText.text = ""+aiPoints;
        playerPointsText.text = ""+playerPoints;
    }

    public void roundManager ()
    {
        ClassBase playerClassScript = player.GetComponent<ClassBase>();
        if (!playerClassScript || playerClassScript.health <= 0)
        {
            aiPoints++;
            curRound++;
            reset ();
        } else if(!enemyClassScript || enemyClassScript.health <= 0) {
            foreach (GameObject enemy in enemies)
            {
                ClassBase script = enemy.GetComponent<ClassBase>();

                if (script && script.health > 0)
                {
                    return;
                }
            }
            playerPoints++;
            curRound++;
            reset ();
        }

        if (aiPoints >= 2 || playerPoints >= 2)
        {
            if (playerPoints > aiPoints)
            {
                Time.timeScale = 0f;
                gameOver = true;
                GameUIText.text = "You Win";
                PlayButtonText.text = "Play Again";
                canvas.transform.gameObject.SetActive(true);
            } else {
                Time.timeScale = 0f;
                gameOver = true;
                GameUIText.text = "You Lose";
                PlayButtonText.text = "Play Again";
                canvas.transform.gameObject.SetActive(true);
            }
        }

    }

    public void reset ()
    {
        foreach (GameObject enemy in enemies)
        {
            ClassBase script = enemy.GetComponent<ClassBase>();
            script.reset ();
        }
        ClassBase playerScript = player.GetComponent<ClassBase>();
        playerScript.reset();      
    }
}
