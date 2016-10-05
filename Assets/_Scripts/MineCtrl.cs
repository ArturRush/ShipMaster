using UnityEngine;
using System.Collections;

public class MineCtrl : MonoBehaviour
{
	public float speed;
	public float tumble;
	public GameObject mineExplosion;
	public GameObject smallExplosion;
	public int cost;
	private GameController gC;
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble; 
		
		GameObject gameControllerObject = GameObject.Find("GameController");
		if (gameControllerObject != null)
		{
			gC = gameControllerObject.GetComponent<GameController>();
		}
		else
		{
			Debug.Log("Cannot find GameController");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (CompareTag("PlayerMine"))
			if (other.CompareTag("EnemyBullet") || other.CompareTag("Explosion") || other.CompareTag("MegaExplosion") || other.CompareTag("EnemyShip") || other.CompareTag("EnemyRocket") || other.CompareTag("Shark") || other.CompareTag("EnemyMine"))
			{
				Instantiate(mineExplosion, transform.position, transform.rotation);
				Instantiate(smallExplosion, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		if (CompareTag("EnemyMine"))
		{
			if (other.CompareTag("PlayerBullet") || other.CompareTag("Shield") || other.CompareTag("Explosion") || other.CompareTag("MegaExplosion") || other.CompareTag("Player") || other.CompareTag("PlayerRocket") || other.CompareTag("PlayerMine"))
			{
				gC.AddScore(cost);
				Instantiate(mineExplosion, transform.position, transform.rotation);
				Instantiate(smallExplosion, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}
	}
}
