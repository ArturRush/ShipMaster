using System;
using UnityEngine;

public class SharkCtrl : MonoBehaviour
{
	public float speed;
	public float horizontalSpeed;
	public int tilt;
	public GameObject sharkExplosion;
	public int cost;
	private Transform player;

	private GameController gC;
	void Start()
	{
		transform.rotation = Quaternion.Euler(-90,180,0);
		GetComponent<Rigidbody>().velocity = -transform.up * speed;
		GameObject gameControllerObject = GameObject.Find("GameController");
		if (gameControllerObject != null)
		{
			gC = gameControllerObject.GetComponent<GameController>();
		}
		else
		{
			Debug.Log("Cannot find GameController");
		}
		GameObject playerTemp = GameObject.Find("PlayerShip");
		if (playerTemp != null)
		{
			player = playerTemp.GetComponent<Transform>();
		}
		else
		{
			Debug.Log("Cannot find player");
		}
	}

	// Update is called once per frame
	void Update()
	{
		try
		{
			if (Mathf.Abs(transform.position.x - player.transform.position.x) > 0.2)
				ChangeDirection(transform.position.x - player.position.x);
		}
		catch(Exception)
		{}
		GetComponent<Rigidbody>().rotation = Quaternion.Euler(-90, 180, GetComponent<Rigidbody>().velocity.x * -tilt);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Explosion") || other.CompareTag("Shield") || other.CompareTag("MegaExplosion") || other.CompareTag("Player") || other.CompareTag("PlayerRocket") || other.CompareTag("PlayerMine") || other.CompareTag("PlayerBullet"))
		{
			gC.AddScore(cost);
			Instantiate(sharkExplosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}

	void ChangeDirection(float moveTo)
	{
		GetComponent<Rigidbody>().velocity = transform.right * horizontalSpeed * Mathf.Sign(moveTo) + -transform.up * speed;
	}
}
