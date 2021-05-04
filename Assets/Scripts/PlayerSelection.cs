using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] menuClassesArr;
    public GameObject[] menuEnemiesOneArr;
    public GameObject[] menuEnemiesTwoArr;
    public GameObject[] menuEnemiesThreeArr;
    public GameObject[] classesArr;
    public GameObject[] enemiesArr;
    public Text description;
    public GameObject errorMsg;
    public Button[] enemyButtons;


    private int enemyNum;
    
    private GameObject currentSelection;
    private GameObject[] enemiesCurrentSelection = new GameObject[3];
    private string[] textArr = {
              "Stats:\n"
            + "\n"
            + "Attack: 15                           Normal Attack: Throws an arrow\n"
            + "Defense: 20                        Heavy Attack: Throws a giant arrow\n"
            + "Health: 100                         Special Attack: Rain of flaming arrows\n"
            + "Attack Range: 200",

              "Stats:\n"
            + "\n"
            + "Attack: 20                           Normal Attack: Swings sword\n"
            + "Defense: 30                        Heavy Attack: Throws sword\n"
            + "Health: 100                         Special Attack: Creates sword of flames\n"
            + "Attack Range: 10",

              "Stats:\n"
            + "\n"
            + "Attack: 15                           Normal Attack: Casts a fireball\n"
            + "Defense: 10                        Heavy Attack: Casts a knockback spell\n"
            + "Health: 100                         Special Attack: Casts a Snare spell\n"
            + "Attack Range: 100",

              "Stats:\n"
            + "\n"
            + "Attack: 20                           Normal Attack: Smashes down sword\n"
            + "Defense: 40                        Heavy Attack: Smash with more force\n"
            + "Health: 100                         Special Attack: Creates a healing shield\n"
            + "Attack Range: 10",

            "Random Character..."
    };

    public void Start()
    {
        Time.timeScale = 1f;
        currentSelection = menuClassesArr[0];
        enemiesCurrentSelection[0] = menuEnemiesOneArr[0];
        enemiesCurrentSelection[1] = menuEnemiesTwoArr[5];
        enemiesCurrentSelection[2] = menuEnemiesThreeArr[5];
        description.text = textArr[0];
        enemyNum = 0;
    }

    public void EnemyChoice(int enemyChoice)
    {
        Image oldButton = enemyButtons[enemyNum].GetComponent<Image>();
        oldButton.color = new Color32(225, 0, 225, 255);
        enemyNum = enemyChoice;
        Image newButton = enemyButtons[enemyNum].GetComponent<Image>();
        newButton.color = new Color32(80, 0, 80, 255);
    }

    public void EnemySelectClass(int classNum) 
    {
        enemiesCurrentSelection[enemyNum].SetActive(false);

        if (enemyNum == 0)
            enemiesCurrentSelection[enemyNum] = menuEnemiesOneArr[classNum];
        else if (enemyNum == 1)
            enemiesCurrentSelection[enemyNum] = menuEnemiesTwoArr[classNum];
        else if (enemyNum == 2)
            enemiesCurrentSelection[enemyNum] = menuEnemiesThreeArr[classNum];

        enemiesCurrentSelection[enemyNum].SetActive(true);

        if (enemyNum == 0 && classNum != 4 && classNum != 5)
            menuEnemiesOneArr[classNum].GetComponentInChildren<Animator>().SetFloat("Animation Speed", 1f);
        else if (enemyNum == 1 && classNum != 4 && classNum != 5)
            menuEnemiesTwoArr[classNum].GetComponentInChildren<Animator>().SetFloat("Animation Speed", 1f);
        else if (enemyNum == 2 && classNum != 4 && classNum != 5)
             menuEnemiesThreeArr[classNum].GetComponentInChildren<Animator>().SetFloat("Animation Speed", 1f);
    }

    public void PlayerSelectClass(int classNum) 
    {
        currentSelection.SetActive(false);
        currentSelection = menuClassesArr[classNum];
        currentSelection.SetActive(true);
        description.text = textArr[classNum];
        if (classNum != 4)
        {
            menuClassesArr[classNum].GetComponentInChildren<Animator>().SetFloat("Animation Speed", 1f);
        }
    }

    public void StartGame() 
    {
        if (enemiesCurrentSelection[0] == menuEnemiesOneArr[5] &&
            enemiesCurrentSelection[1] == menuEnemiesTwoArr[5] &&
            enemiesCurrentSelection[2] == menuEnemiesThreeArr[5])
        {
            errorMsg.SetActive(true);
            return;
        }


        if (currentSelection == menuClassesArr[0])
        {
            GameManager.playerClass = classesArr[0];
        }
        else if (currentSelection == menuClassesArr[1])
        {
            GameManager.playerClass = classesArr[1];
        }
        else if (currentSelection == menuClassesArr[2])
        {
            GameManager.playerClass = classesArr[2];
        }
        else if (currentSelection == menuClassesArr[3])
        {
            GameManager.playerClass = classesArr[3];
        }
        else if (currentSelection == menuClassesArr[4])
        {
            int choice = Random.Range(0, 4);
            GameManager.playerClass = classesArr[choice];
        }

        // Enemy One
        if (enemiesCurrentSelection[0] == menuEnemiesOneArr[0])
        {
            GameManager.enemyClass = enemiesArr[0];
            GameManager.noEnemyClass = false;
        }
        else if (enemiesCurrentSelection[0] == menuEnemiesOneArr[1])
        {
            GameManager.enemyClass = enemiesArr[1];
            GameManager.noEnemyClass = false;
        }
        else if (enemiesCurrentSelection[0] == menuEnemiesOneArr[2])
        {
            GameManager.enemyClass = enemiesArr[2];
            GameManager.noEnemyClass = false;
        }
        else if (enemiesCurrentSelection[0] == menuEnemiesOneArr[3])
        {
            GameManager.enemyClass = enemiesArr[3];
            GameManager.noEnemyClass = false;
        }
        else if (enemiesCurrentSelection[0] == menuEnemiesOneArr[4])
        {
            int choice = Random.Range(0, 4);
            GameManager.enemyClass = enemiesArr[choice];
            GameManager.noEnemyClass = false;
        }
        else if (enemiesCurrentSelection[0] == menuEnemiesOneArr[5])
        {
            GameManager.noEnemyClass = true;
        }

        // Enemy Two
        if (enemiesCurrentSelection[1] == menuEnemiesTwoArr[0])
        {
            GameManager.enemyTwoClass = enemiesArr[0];
            GameManager.noEnemyTwoClass = false;
        }
        else if (enemiesCurrentSelection[1] == menuEnemiesTwoArr[1])
        {
            GameManager.enemyTwoClass = enemiesArr[1];
            GameManager.noEnemyTwoClass = false;
        }
        else if (enemiesCurrentSelection[1] == menuEnemiesTwoArr[2])
        {
            GameManager.enemyTwoClass = enemiesArr[2];
            GameManager.noEnemyTwoClass = false;
        }
        else if (enemiesCurrentSelection[1] == menuEnemiesTwoArr[3])
        {
            GameManager.enemyTwoClass = enemiesArr[3];
            GameManager.noEnemyTwoClass = false;
        }
        else if (enemiesCurrentSelection[1] == menuEnemiesTwoArr[4])
        {
            int choice = Random.Range(0, 4);
            GameManager.enemyTwoClass = enemiesArr[choice];
            GameManager.noEnemyTwoClass = false;
        }
        else if (enemiesCurrentSelection[1] == menuEnemiesTwoArr[5])
        {
            GameManager.noEnemyTwoClass = true;
        }

        // Enemy Three
        if (enemiesCurrentSelection[2] == menuEnemiesThreeArr[0])
        {
            GameManager.enemyThreeClass = enemiesArr[0];
            GameManager.noEnemyThreeClass = false;
        }
        else if (enemiesCurrentSelection[2] == menuEnemiesThreeArr[1])
        {
            GameManager.enemyThreeClass = enemiesArr[1];
            GameManager.noEnemyThreeClass = false;
        }
        else if (enemiesCurrentSelection[2] == menuEnemiesThreeArr[2])
        {
            GameManager.enemyThreeClass = enemiesArr[2];
            GameManager.noEnemyThreeClass = false;
        }
        else if (enemiesCurrentSelection[2] == menuEnemiesThreeArr[3])
        {
            GameManager.enemyThreeClass = enemiesArr[3];
            GameManager.noEnemyThreeClass = false;
        }
        else if (enemiesCurrentSelection[2] == menuEnemiesThreeArr[4])
        {
            int choice = Random.Range(0, 4);
            GameManager.enemyThreeClass = enemiesArr[choice];
            GameManager.noEnemyThreeClass = false;
        }
        else if (enemiesCurrentSelection[2] == menuEnemiesThreeArr[5])
        {
            GameManager.noEnemyThreeClass = true;
        }
        
        SceneManager.LoadScene("PlayScene");
    }

    public void closeError()
    {
        errorMsg.SetActive(false);
    }
}