using UnityEngine;
using System.Collections;

public class AdditiveCamCtrl : MonoBehaviour
{
	public Transform player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.rotation = Quaternion.Euler(30 20, 10);
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
		transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
	}
}
