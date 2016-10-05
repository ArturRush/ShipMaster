using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{
	public float lifeTime;
	private void Start()
	{
		Destroy(gameObject, lifeTime);
	}
}
