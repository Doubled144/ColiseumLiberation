using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

	public GameObject canvas;

	public void Play ()
	{
		if (GameManager.gameOver)
		{
			SceneManager.LoadScene("Player Selection");
		}
		else
		{
			Time.timeScale = 1f;
			canvas.transform.gameObject.SetActive(false);
		}
	}
	

	public void LoadMainMenu ()
	{
		SceneManager.LoadScene("Main Menu");
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
