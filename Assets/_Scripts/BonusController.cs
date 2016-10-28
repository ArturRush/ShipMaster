using UnityEngine;
using System.Collections;

public class BonusController : MonoBehaviour
{


	public float speed;
	public float tumble;
	private PlayerController player;
	private GameController gameController;
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;

		GameObject gameControllerObject = GameObject.Find("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		else
		{
			Debug.Log("Cannot find GameController");
		}

		GameObject playerObject = GameObject.Find("PlayerShip");
		if (playerObject != null)
		{
			player = playerObject.GetComponent<PlayerController>();
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			switch (name)
			{
				case "DoublePoints(Clone)":
					{
						gameController.UseBonusDoublePoints();
					}
					break;
				case "MegaMine(Clone)":
					{
						gameController.UseBonusMegaMine();
					}
					break;
				case "RocketStrike(Clone)":
					{
						gameController.UseBonusRocketStrike();
					}
					break;
				case "InfinityAmmo(Clone)":
					{
						player.UseBonusInfAmmo();
					}
					break;
				case "Shield(Clone)":
					{
						player.UseBonusShield();
					}
					break;
			}
			Destroy(gameObject);
		}
	}
}
