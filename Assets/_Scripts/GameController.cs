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
			return new Leaders { name = tmp[0], scorePts = int.Parse(tmp[1]), level = Int32.Parse(tmp[2].Trim('(')) };
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
	//public Text gameOverText;
	public Text lvlText;
	public Text lifesText;
	public Image shieldBonusImg;
	public Image doublePtsBonusImg;
	public Image infAmmoBonusImg;

	private bool gameOver;

	public GameObject megaMine;
	public GameObject rocket;

	public GameObject goPanel;
	public GameObject canvas;

	public AudioClip bonusClip;

	public GameObject PauseBtn;
	public GameObject PausePanel;

	private bool doublePoints;
	public float doublePointsDuration;
	private int lvl;
	private string leadersFile = "Score.txt";

	private List<float> hazardProbability = new List<float>();

	private List<Leaders> leaderBoard = new List<Leaders>();
	void Start()
	{
		StartCoroutine(SpawnWaves());
		StartCoroutine(SpawnBonuses());
		gameOver = false;

		doublePoints = false;
		doublePtsBonusImg.fillAmount = 0;
		shieldBonusImg.fillAmount = 0;
		infAmmoBonusImg.fillAmount = 0;
		restartText.text = "";
		//gameOverText.text = "";
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

		hazardProbability.Add(1);
		hazardProbability.Add(0);
		hazardProbability.Add(0);
	}

	void Update()
	{
		if (gameOver)
		{
			gameOver = false;
			Instantiate(goPanel, new Vector3(canvas.transform.position.x, 0, 0), new Quaternion(0,0,0,0),canvas.transform);
			DisablePause();
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
				float r = Random.value;
				if (r < hazardProbability[0])//Акулы
					hazard = hazards[0];
				if (hazardProbability[0] <= r && r < hazardProbability[1] + hazardProbability[0])//Мины
					hazard = hazards[1];
				if (hazardProbability[1] + hazardProbability[0] <= r)//Катера
					hazard = hazards[2];
				if (hazard == null) yield return new WaitForSeconds(Random.Range(0.2f, Mathf.Max(0.2f, spawnWait - (lvl / 50))));
				Vector3 spawnPosition = new Vector3(Random.Range(-spawnPos.x, spawnPos.x), hazard == hazards[0] ? spawnPos.y - 0.2f : spawnPos.y, spawnPos.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate(hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(Random.Range(0.2f, Mathf.Max(0.2f, spawnWait - (lvl / 50))));
			}
			//Увеличение сложности с повышением уровня
			//После 30 уровня сложность постоянна
			//Дойти до 30 уровня - сложно
			if (lvl < 20)
			{
				hazardProbability[0] -= 0.03f;
				hazardProbability[1] += 0.02f;
				hazardProbability[2] += 0.01f;
			}
			if (lvl >= 20 && lvl < 30)
			{
				hazardProbability[0] -= 0.01f;
				hazardProbability[1] -= 0.01f;
				hazardProbability[2] += 0.02f;
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
		gameOver = true;
		//AddToLeaders();
	}

	public void AddScore(int newScoreValue)
	{
		if (gameOver)
			return;
		score += newScoreValue + lvl * scoreMultiplier * newScoreValue;
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
		leaderBoard.Add(new Leaders() { name = playerName, scorePts = (int)score, level = lvl });
		leaderBoard = leaderBoard.OrderByDescending(x => x.scorePts).ToList();
		string[] tmp = new string[leaderBoard.Count];
		//Debug.Log("============Leaders============");
		for (int i = 0; i < Mathf.Min(10, leaderBoard.Count); ++i)
		{
			tmp[i] = leaderBoard[i].name + ' ' + leaderBoard[i].scorePts + " (" + leaderBoard[i].level + " волна)";
			//Debug.Log(leaderBoard[i].name + ' ' + leaderBoard[i].scorePts + " (" + leaderBoard[i].level + " волна)");
		}
		File.WriteAllText(leadersFile, "");
		File.WriteAllLines(leadersFile, tmp);
	}

	public void UseBonusDoublePoints()
	{
		GetComponent<AudioSource>().PlayOneShot(bonusClip);
		doublePtsBonusImg.fillAmount = 1;
		doublePoints = true;
	}

	public void UseBonusMegaMine()
	{
		GetComponent<AudioSource>().PlayOneShot(bonusClip);
		Instantiate(megaMine, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
	}

	public void UseBonusRocketStrike()
	{
		GetComponent<AudioSource>().PlayOneShot(bonusClip);
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

	private void DisablePause()
	{
		PauseBtn.SetActive(false);
		PausePanel.SetActive(false);
	}
}
