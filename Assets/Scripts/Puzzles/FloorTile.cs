using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
	[SerializeField]
	private Material tileCorrect;

	[SerializeField]
    private Transform respawnPoint;

	[HideInInspector]
	public bool safe = false;

	private MeshRenderer renderer;

	void Start()
	{
		renderer = GetComponent<MeshRenderer>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (!safe)
			{
				other.transform.position = respawnPoint.position;
			}
			else
			{
				renderer.material = tileCorrect;
			}
		}
	}
}
