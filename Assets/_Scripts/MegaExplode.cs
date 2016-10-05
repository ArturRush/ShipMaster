using UnityEngine;
using System.Collections;

public class MegaExplode : MonoBehaviour
{
	public GameObject megaExplosion;
	void Start ()
	{
		Instantiate(megaExplosion, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
	}
}
