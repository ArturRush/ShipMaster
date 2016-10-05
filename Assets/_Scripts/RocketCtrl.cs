using UnityEngine;

public class RocketCtrl : MonoBehaviour
{
	public float speed;
	public float tumble;
	public GameObject rocketExplosion;
	public int cost;
	private GameController gC;
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
		if (CompareTag("EnemyRocket"))
			transform.Rotate(new Vector3(0, 180, 0));
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

	void Update()
	{
		transform.Rotate(new Vector3(0, 0, tumble));
	}

	void OnTriggerEnter(Collider other)
	{
		if (CompareTag("PlayerRocket"))
			if (other.CompareTag("EnemyBullet") || other.CompareTag("Explosion") || other.CompareTag("MegaExplosion") || other.CompareTag("EnemyShip") || other.CompareTag("EnemyRocket") || other.CompareTag("Shark") || other.CompareTag("EnemyMine"))
			{
				Instantiate(rocketExplosion, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		if (CompareTag("EnemyRocket"))
		{
			if (other.CompareTag("PlayerBullet") || other.CompareTag("Shield") || other.CompareTag("Explosion") || other.CompareTag("MegaExplosion") || other.CompareTag("Player") || other.CompareTag("PlayerRocket") || other.CompareTag("PlayerMine"))
			{
				gC.AddScore(cost);
				Instantiate(rocketExplosion, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}
	}
}
