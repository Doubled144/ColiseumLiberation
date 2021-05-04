using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject controlWindow;
	public GameObject creditMenu;

	public void Start ()
	{
		Time.timeScale = 1f;
	}

	public void LoadGame ()
	{
		SceneManager.LoadScene("Player Selection");
	}

	public void doControlsMenu()
	{
		controlWindow.SetActive(true);
	}

	public void closeControlsMenu()
	{
		controlWindow.SetActive(false);
	}

	public void doCreditMenu()
	{
		creditMenu.SetActive(true);
	}

	public void closeCreditMenu()
	{
		creditMenu.SetActive(false);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
