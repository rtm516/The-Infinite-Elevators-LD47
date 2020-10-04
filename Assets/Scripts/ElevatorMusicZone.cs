using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMusicZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameManager.Instance.elevatorMusic.transform.position = transform.position;
		}
	}
}
