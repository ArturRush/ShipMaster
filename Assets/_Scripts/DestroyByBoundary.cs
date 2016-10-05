using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
	void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Player"))// && !other.CompareTag("Shield"))
		{
			Destroy(other.gameObject);
		}
	}
}
