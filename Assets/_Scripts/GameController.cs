using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct Leaders
{
	public string name;
	public int scorePts;

	public static Leaders Parse(string line)
	{
		try
		{
			string[] tmp = line.Split(' ');
			return new Leaders { name = tmp[0], scorePts = int.Parse(tmp[1]) };
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
	public int score;

	public Text restartText;
	public Text gameOverText;

	private bool gameOver;
	private bool restart;

	public GameObject megaMine;
	public GameObject rocket;

	private bool doublePoints;
	public float doublePointsDuration;
	private Timer dPointsTimer;
	private int lvl;


	private List<Leaders> leaderBoard = new List<Leaders>();
	void Start () {
		StartCoroutine(SpawnWaves());
		StartCoroutine(SpawnBonuses());
		gameOver = false;
		restart = false;

		doublePoints = false;

		restartText.text = "";
		gameOverText.text = "";
		UpdateScore();
		lvl = 1;
		playerName = "Artur";
		if (!File.Exists("ScoreAlfa.txt"))
			File.WriteAllText("ScoreAlfa.txt", "");
		string[] str = File.ReadAllLines("ScoreAlfa.txt");
		foreach (var s in str)
		{
			Leaders temp = Leaders.Parse(s);
			leaderBoard.Add(temp);
		}

		dPointsTimer = new Timer(doublePointsDuration * 1000);
		dPointsTimer.Elapsed += dPointsTimer_Elapsed;
	}
	
	void Update () {
		if (restart && Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if (gameOver)
		{
			restartText.text = "Press 'R' to restart game";
			restart = true;
		}
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startWait);
		while (true)
		{
			Debug.Log("Текущий уровень:" + lvl);
			for (int i = 0; i < Mathf.Max(4, (int)(0.5* lvl * hazardCount)); ++i)
			{
				GameObject hazard = null;
				if(lvl<2)
					hazard = hazards[0];	//Акулы
				else if(lvl<5)
					hazard = hazards[Random.Range(0, 2)];//Акулы и мины
				else
					hazard = hazards[Random.Range(0, hazards.Length)];//Появляются катера
				Vector3 spawnPosition = new Vector3(Random.Range(-spawnPos.x, spawnPos.x), spawnPos.y, spawnPos.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate(hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(Random.Range(0.2f, Mathf.Max(0.2f, spawnWait - (lvl / 50))));
			}
			++lvl;
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
				yield return new WaitForSeconds(Random.Range(bonusWait/5, bonusWait));
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
		score += newScoreValue;
		if (doublePoints)
			score += newScoreValue;
		UpdateScore();
	}

	public void TakeScore(int toTake)
	{
		score -= toTake;
		if (score < 0)
			score = 0;
		UpdateScore();
	}

	public bool CanShoot(int cost)
	{
		return score - cost >= 0;
	}

	void UpdateScore()
	{
		scoreText.text = "Score: " + score;
	}

	public void AddToLeaders()
	{
		leaderBoard.Add(new Leaders() { name = playerName, scorePts = score });
		leaderBoard = leaderBoard.OrderByDescending(x => x.scorePts).ToList();
		string[] tmp = new string[leaderBoard.Count];
		Debug.Log("============Leaders============");
		for (int i = 0; i < Mathf.Min(10, leaderBoard.Count); ++i)
		{
			tmp[i] = leaderBoard[i].name + ' ' + leaderBoard[i].scorePts;
			Debug.Log(leaderBoard[i].name + ' ' + leaderBoard[i].scorePts);
		}
		File.WriteAllLines("ScoreAlfa.txt", tmp);
	}

	public void UseBonusDoublePoints()
	{
		if (doublePoints)
		{
			dPointsTimer.Stop();
			dPointsTimer.Start();
		}
		else
		{
			doublePoints = true;
			dPointsTimer.Start();
		}
	}

	void dPointsTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		dPointsTimer.Stop();
		doublePoints = false;
	}

	public void UseBonusMegaMine()
	{
		Instantiate(megaMine, new Vector3(0,0,0), new Quaternion(0,0,0,0));
	}

	public void UseBonusRocketStrike()
	{
		for (float i = -3.8f; i <= 4.2f; i += 1.2f)
		{
			Instantiate(rocket, new Vector3(i, spawnPos.y, -7.0f),
				Quaternion.Euler(0, 0, 0));
		}
		for (float i = -3.8f; i <= 4.2f; i+= 1.2f)
		{
			Instantiate(rocket, new Vector3(i, spawnPos.y, 7.0f),
				Quaternion.Euler(0, 180, 0));
		}
	}
}
