using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuScript : MonoBehaviour
{
	public Text PlayerName;
	private GameController gameController;

	void Start()
	{
		GameObject gameControllerObject = GameObject.Find("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		else
		{
			Debug.Log("Cannot find GameController");
		}
	}
	
	public void RestartGame()
	{
		SaveRecord();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}


	public void ToMainMenuClick()
	{
		SaveRecord();
		SceneManager.LoadScene(1);//Индекс сцены главного меню
	}

	public void ExitGame()
	{
		SaveRecord();
		Application.Quit();
	}

	private void SaveRecord()
	{
		gameController.playerName = PlayerName.text == ""
			? "Моряк"
			: PlayerName.text;
		gameController.AddToLeaders();
	}
}
