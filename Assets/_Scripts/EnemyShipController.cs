using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
	public float speed;
	public float horizontalSpeed;
	public GameObject enemyExplosion;
	public GameObject smallExplosion;
	public int cost;
	public GameObject Rocket;
	public float dirChangeTime;
	private float nextDir;
	public float fireRate;
	private float nextFire;

	private GameController gC;
	void Start()
	{
		transform.Rotate(new Vector3(0, 180, 0));
		GetComponent<Rigidbody>().velocity = transform.forward * speed;

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
		if (Time.time > nextFire)
		{
			nextFire = Time.time + Random.Range(fireRate / 2, fireRate * 2);
			ShootRocket();
		}
		if (Time.time > nextDir)
		{
			nextDir = Time.time + Random.Range(dirChangeTime / 2, dirChangeTime * 2);

			ChangeDirection(Random.Range(-4.2f, 4.2f));
		}
		GetComponent<Rigidbody>().position = new Vector3
			(Mathf.Clamp(GetComponent<Rigidbody>().position.x, -4.2f, 4.2f), 0.0f, Mathf.Clamp(GetComponent<Rigidbody>().position.z, -100, 100));
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Explosion") || other.CompareTag("Shield") || other.CompareTag("MegaExplosion") || other.CompareTag("Player") || other.CompareTag("PlayerRocket") || other.CompareTag("PlayerMine"))
		{
			gC.AddScore(cost);
			Instantiate(enemyExplosion, transform.position, transform.rotation);
			Instantiate(smallExplosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}

	void ShootRocket()
	{
		//0.222 - потому что такова высота полета пули игрока
		Instantiate(Rocket, new Vector3(transform.position.x, 0.222f, transform.position.z - 1f), new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w));
	}

	void ChangeDirection(float moveTo)
	{
		GetComponent<Rigidbody>().velocity = transform.right * horizontalSpeed * Mathf.Sign(moveTo) + transform.forward * speed;
	}
}
