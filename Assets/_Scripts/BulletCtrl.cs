using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
	public float speed;
	public int cost;
	private GameController gC;
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
		if (CompareTag("EnemyBullet"))
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

	void OnTriggerEnter(Collider other)
	{
		if(CompareTag("PlayerBullet"))
			if (other.CompareTag("EnemyBullet") || other.CompareTag("Explosion") || other.CompareTag("MegaExplosion") || other.CompareTag("EnemyShip") || other.CompareTag("EnemyRocket") || other.CompareTag("Shark") || other.CompareTag("EnemyMine"))
			{
				Destroy(gameObject);
			}
		if (CompareTag("EnemyBullet"))
		{
			if (other.CompareTag("PlayerBullet") || other.CompareTag("Explosion") || other.CompareTag("MegaExplosion") || other.CompareTag("Player") || other.CompareTag("PlayerRocket") || other.CompareTag("PlayerMine") || other.CompareTag("Shield"))
			{
				gC.AddScore(cost);
				Destroy(gameObject);
			}
		}
	}
}
