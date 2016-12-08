using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
