using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
	public void ContinuClick()
	{
		GetComponent<RectTransform>().anchoredPosition = new Vector2(1000,0);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	public void ToMainMenuClick()
	{
		SceneManager.LoadScene(1);//Индекс сцены главного меню
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void OpenPausePanelClick()
	{
		GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
	}
}
