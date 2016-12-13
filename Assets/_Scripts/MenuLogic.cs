using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
	public Text NamesColumn;
	public Text LevelColumn;
	public Text ScoreColumn;
	void Start()
	{
		if (!File.Exists("Score.txt"))
			File.WriteAllText("Score.txt", "");
		string[] str = File.ReadAllLines("Score.txt");
		for(int i = 0; i<Mathf.Min(10, str.Length);++i)
		{
			Leaders temp = Leaders.Parse(str[i]);
			if (temp.level == 0 && temp.scorePts == 0) break;
			NamesColumn.text += temp.name + "\n";
			LevelColumn.text += temp.level + "\n";
			ScoreColumn.text += temp.scorePts + "\n";
		}
	}

	public void NewGameClick()
	{
		SceneManager.LoadScene(0);//0 - индекс сцены с игрой
	}

	public void RecordsClick(GameObject recPanel)
	{
		recPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
	}

	public void RecordsCloseClick(GameObject recPanel)
	{
		recPanel.transform.position = new Vector3(2000,0,0);
	}

	public void ExitClick()
	{
		Application.Quit();
	}
}
