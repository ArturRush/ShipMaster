using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public struct Leaders
{
	public string name;
	public int scorePts;
	public int level;

	public static Leaders Parse(string line)
	{
		try
		{
			string[] tmp = line.Split(' ');
			return new Leaders { name = tmp[0], scorePts = int.Parse(tmp[1]) , level = Int32.Parse(tmp[2].Trim('('))};
		}
		catch
		{
			return new Leaders();
		}
	}
}

public class GameController : MonoBehaviour
{

	public Vector3 spawnPos;
	public GameObject[] hazards;
	public float hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public string playerName;

	public GameObject[] bonuses;
	public float bonusWait;
	public float bounusStartTime;


	public Text scoreText;
	public float score;
	public float scoreMultiplier;//На это значение умножается номер уровня и добавляется к каждому полученному очку

	public Text restartText;
	public Text gameOverText;
	public Text lvlText;
	public Text lifesText;
	public Image shieldBonusImg;
	public Image doublePtsBonusImg;
	public Image infAmmoBonusImg;

	private bool gameOver;
	private bool restart;

	public GameObject megaMine;
	public GameObject rocket;

	private bool doublePoints;
	public float doublePointsDuration;
	private int lvl;
	private string leadersFile = "Score.txt";

	private List<int> hazardProbability;
	private List<int> bonusProbability;

	private List<Leaders> leaderBoard = new List<Leaders>();
	void Start()
	{
		StartCoroutine(SpawnWaves());
		StartCoroutine(SpawnBonuses());
		gameOver = false;
		restart = false;

		doublePoints = false;
		doublePtsBonusImg.fillAmount = 0;
		shieldBonusImg.fillAmount = 0;
		infAmmoBonusImg.fillAmount = 0;
		restartText.text = "";
		gameOverText.text = "";
		UpdateScoreText();
		lvl = 1;
		UpdateLvlText();
		if (!File.Exists(leadersFile))
			File.WriteAllText(leadersFile, "");
		string[] str = File.ReadAllLines(leadersFile);
		foreach (var s in str)
		{
			Leaders temp = Leaders.Parse(s);
			leaderBoard.Add(temp);
		}
	}

	void Update()
	{
		if (restart && Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if (gameOver)
		{
			restartText.text = "Press 'R' to restart game";
			restart = true;
		}
		if (doublePoints)
		{
			doublePtsBonusImg.fillAmount -= 1f / doublePointsDuration * Time.deltaTime;
			if (doublePtsBonusImg.fillAmount < 0.00001)
				doublePoints = false;
		}
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startWait);
		while (true)
		{
			for (int i = 0; i < Mathf.Max(4, (int)(0.5 * lvl * hazardCount)); ++i)
			{
				GameObject hazard = null;
				if (lvl < 2)
					hazard = hazards[0];	//Акулы
				else if (lvl < 5)
					hazard = hazards[Random.Range(0, 2)];//Акулы и мины
				else
					hazard = hazards[Random.Range(0, hazards.Length)];//Появляются катера
				Vector3 spawnPosition = new Vector3(Random.Range(-spawnPos.x, spawnPos.x), spawnPos.y, spawnPos.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate(hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(Random.Range(0.2f, Mathf.Max(0.2f, spawnWait - (lvl / 50))));
			}
			++lvl;
			UpdateLvlText();
			yield return new WaitForSeconds(waveWait);
		}
	}

	IEnumerator SpawnBonuses()
	{
		yield return new WaitForSeconds(bounusStartTime);
		while (true)
		{
			GameObject bon = bonuses[Random.Range(0, bonuses.Length)];
			Vector3 spawnPosition = new Vector3(Random.Range(-spawnPos.x, spawnPos.x), spawnPos.y, spawnPos.z);
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate(bon, spawnPosition, spawnRotation);
			yield return new WaitForSeconds(Random.Range(bonusWait / 5, bonusWait));
		}
	}

	public void GameOver()
	{
		gameOverText.text = "GAME OVER!";
		gameOver = true;
		AddToLeaders();
	}

	public void AddScore(int newScoreValue)
	{
		if (gameOver)
			return;
		score += newScoreValue + lvl*scoreMultiplier*newScoreValue;
		if (doublePoints)
			score += newScoreValue + lvl * scoreMultiplier * newScoreValue;
		UpdateScoreText();
	}

	public void TakeScore(int toTake)
	{
		score -= toTake;
		if (score < 0)
			score = 0;
		UpdateScoreText();
	}

	public bool CanShoot(int cost)
	{
		return score - cost >= 0;
	}

	void UpdateScoreText()
	{
		scoreText.text = "Score: " + (int)score;
	}

	public void UpdateLifesText(int lifes)
	{
		lifesText.text = "Lifes: " + lifes;
	}

	void UpdateLvlText()
	{
		lvlText.text = "Wave: " + lvl;
	}

	public void UpdateShieldPict(float fillAmount)
	{
		shieldBonusImg.fillAmount = fillAmount;
	}

	public void UpdateInfAmmoPict(float fillAmount)
	{
		infAmmoBonusImg.fillAmount = fillAmount;
	}

	public void AddToLeaders()
	{
		leaderBoard.Add(new Leaders() { name = playerName, scorePts = (int)score , level = lvl});
		leaderBoard = leaderBoard.OrderByDescending(x => x.scorePts).ToList();
		string[] tmp = new string[leaderBoard.Count];
		Debug.Log("============Leaders============");
		for (int i = 0; i < Mathf.Min(10, leaderBoard.Count); ++i)
		{
			tmp[i] = leaderBoard[i].name + ' ' + leaderBoard[i].scorePts + " (" + leaderBoard[i].level + " уровень)";
			Debug.Log(leaderBoard[i].name + ' ' + leaderBoard[i].scorePts + " (" + leaderBoard[i].level + " уровень)");
		}
		File.WriteAllText(leadersFile, "");
		File.WriteAllLines(leadersFile, tmp);
	}

	public void UseBonusDoublePoints()
	{
		doublePtsBonusImg.fillAmount = 1;
		doublePoints = true;
	}

	public void UseBonusMegaMine()
	{
		Instantiate(megaMine, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
	}

	public void UseBonusRocketStrike()
	{
		for (float i = -3.8f; i <= 4.2f; i += 1.2f)
		{
			Instantiate(rocket, new Vector3(i, spawnPos.y, -7.0f),
				Quaternion.Euler(0, 0, 0));
		}
		for (float i = -3.8f; i <= 4.2f; i += 1.2f)
		{
			Instantiate(rocket, new Vector3(i, spawnPos.y, 7.0f),
				Quaternion.Euler(0, 180, 0));
		}
	}
}
