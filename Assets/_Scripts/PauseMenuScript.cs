using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
	private bool pause = false;
	private int counter = 30;
	void Update()
	{
		if (counter > -2) counter--;
		if (Input.GetKey(KeyCode.Escape) && counter <= 0)
		{
			counter = 30;
			if (pause)
			{
				ContinueClick();
			}
			else
			{
				OpenPausePanelClick();
			}
		}
	}

	public void ContinueClick()
	{
		GetComponent<RectTransform>().anchoredPosition = new Vector2(1000,0);
		pause = false;
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	public void ToMainMenuClick()
	{
		SceneManager.LoadScene(0);//Индекс сцены главного меню
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void OpenPausePanelClick()
	{
		GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
		pause = true;
	}
}
