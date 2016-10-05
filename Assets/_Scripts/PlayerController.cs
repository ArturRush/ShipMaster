using UnityEngine;
using Timer = System.Timers.Timer;
using Random = UnityEngine.Random;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;//3.25X, -5 to 3 Z	
}

public class PlayerController : MonoBehaviour
{
	public Boundary boundary;
	public float speed;
	public float tilt;

	public int cost;

	public GameObject bullet;
	public GameObject mine;
	public GameObject rocket;
	public GameObject shield;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire;

	private bool InfAmmo;
	private Timer infAmmoTimer;
	public float infAmmoDur;
	public float infAmmoDelay;
	private float infAmmoNow;
	private bool shieldBonus;
	public float shieldBonusDur;
	private float shieldTime;
	
	public GameObject playerExplosion;
	public GameController gameController;

	private void Start()
	{
		this.transform.Rotate(new Vector3(270, 180, 0));
		InfAmmo = false;
		infAmmoTimer = new Timer(1000* infAmmoDur);
		infAmmoTimer.Elapsed += infAmmoTimer_Elapsed;
		shieldBonus = false;
		shieldTime = shieldBonusDur;
		HideShield();
	}
	
	private void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(speed * moveHorizontal, 0.0f, moveVertical * speed);
		GetComponent<Rigidbody>().velocity = movement;

		GetComponent<Rigidbody>().position = new Vector3
			(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
			);
		GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}

	private void Update()
	{
		if (shieldBonus)
		{
			shieldTime -= Time.fixedDeltaTime;
		}
		if (shieldTime <= 0 && shieldBonus)
		{
			shieldBonus = false;
			HideShield();
			shieldTime = shieldBonusDur;
		}

		if (Input.GetButton("Jump") && Time.time > nextFire)
		{
			if (gameController.CanShoot(cost))
			{
				nextFire = Time.time + fireRate;
				gameController.TakeScore(cost);
				Bullet();
			}
		}
		if (Input.GetKeyDown(KeyCode.E) && Time.time > nextFire)
		{
			if (gameController.CanShoot(cost*15))
			{
				nextFire = Time.time + fireRate;
				gameController.TakeScore(cost*15);
				Mine();
			}
		}
		if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextFire)
		{
			if (gameController.CanShoot(cost*10))
			{
				nextFire = Time.time + fireRate;
				gameController.TakeScore(cost*10);
				Rocket();
			}
		}
		if (InfAmmo && Time.time > infAmmoNow)
		{
			infAmmoNow = Time.time + infAmmoDelay;
			switch (Random.Range(0,2))
			{
				case 0:
					Bullet(Random.Range(-30,30));
					break;
				case 1: 
					Rocket();
					break;
			}
		}
	}

	public void Bullet(float direction = 0)
	{
		Instantiate(bullet, shotSpawn.position,
			Quaternion.Euler(shotSpawn.rotation.x, shotSpawn.rotation.y + direction, shotSpawn.rotation.z));
		//GetComponent<AudioSource>().Play();
	}

	public void Mine(float direction = 0)
	{
		Instantiate(mine, shotSpawn.position,
			Quaternion.Euler(shotSpawn.rotation.x, shotSpawn.rotation.y + direction, shotSpawn.rotation.z));
		//GetComponent<AudioSource>().Play();
	}

	public void Rocket(float direction = 0)
	{
		Instantiate(rocket, shotSpawn.position,
			Quaternion.Euler(shotSpawn.rotation.x, shotSpawn.rotation.y + direction, shotSpawn.rotation.z));
		//GetComponent<AudioSource>().Play();
	}

	void OnTriggerEnter(Collider other)
	{
		if (!shieldBonus && (other.CompareTag("Explosion") || other.CompareTag("EnemyShip") || other.CompareTag("EnemyRocket") || other.CompareTag("Shark") || other.CompareTag("EnemyMine")))
		{
			Instantiate(playerExplosion, transform.position, transform.rotation);
			gameController.GameOver();
			Destroy(gameObject);
		}
	}

	public void UseBonusInfAmmo()
	{
		if (InfAmmo)
		{
			infAmmoTimer.Stop();
			infAmmoTimer.Start();
		}
		else
		{
			InfAmmo = true;
			infAmmoTimer.Start();
		}
	}


	void infAmmoTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		infAmmoTimer.Stop();
		InfAmmo = false;
	}

	public void UseBonusShield()
	{
		if (shieldBonus)
		{
			shieldTime = shieldBonusDur;
		}
		else
		{
			shieldBonus = true;
			ShowShield();
		}
	}

	void ShowShield()
	{
		shield.GetComponent<Renderer>().enabled = true;
		shield.GetComponent<Collider>().enabled = true;
	}

	void HideShield()
	{
		shield.GetComponent<Renderer>().enabled = false;
		shield.GetComponent<Collider>().enabled = false;
	}
}