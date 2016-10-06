using UnityEngine;
using System.Collections;

public class StrikeCtrl : MonoBehaviour {
	public float speed;
	public float tumble;
	public GameObject rocketExplosion;
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	void Update()
	{
		transform.Rotate(new Vector3(0, 0, tumble));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (CompareTag("PlayerRocket"))
			if (other.CompareTag("EnemyBullet") || other.CompareTag("Explosion") || other.CompareTag("MegaExplosion") ||
			    other.CompareTag("EnemyShip") || other.CompareTag("EnemyRocket") || other.CompareTag("Shark") ||
			    other.CompareTag("EnemyMine"))
			{
				Instantiate(rocketExplosion, transform.position, transform.rotation);
				Destroy(gameObject);
			}
	}
}
