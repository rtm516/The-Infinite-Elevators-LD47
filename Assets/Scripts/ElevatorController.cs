using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public static ElevatorController mainElevator;

    [SerializeField]
    private ElevatorDoor leftDoor;

    [SerializeField]
    private ElevatorDoor rightDoor;

    [SerializeField]
    private AudioSource bell; 

    private bool notMain = false;
    private bool playerInside = true;

    // Start is called before the first frame update
    void Start()
    {
        if (mainElevator == null)
		{
            mainElevator = this;
        }

        if (this != mainElevator)
		{
            StartCoroutine(OpenDoors(true));
            notMain = true;
		}
    }

    // Update is called once per frame
    void Update()
    {

    }

	private void OnTriggerEnter(Collider other)
	{
        if (notMain && other.CompareTag("Player"))
        {
            playerInside = true;
            StartCoroutine(ResetLevel(other.gameObject));
        }
    }

	private void OnTriggerExit(Collider other)
	{
        if (notMain && other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

	IEnumerator ResetLevel(GameObject player)
    {
        yield return CloseDoors();

        if (!playerInside)
        {
            yield return OpenDoors();
            yield break;
		}

        yield return mainElevator.CloseDoors(true);
        TeleportToMain(player);

        yield return GameManager.Instance.GenerateFloor(gameObject);
    }

    public IEnumerator OpenDoors(bool force = false)
	{
        if (!force)
		{
            bell.Play();
        }

        StartCoroutine(leftDoor.OpenDoor(force));
        StartCoroutine(rightDoor.OpenDoor(force));

        yield return new WaitForSeconds(2f);
    }

    public IEnumerator CloseDoors(bool force = false)
    {
        StartCoroutine(leftDoor.CloseDoor(force));
        StartCoroutine(rightDoor.CloseDoor(force));

        yield return new WaitForSeconds(2f);
    }

    private void TeleportToMain(GameObject player)
	{
        Vector3 posOffset = transform.InverseTransformPoint(player.transform.position);
        Quaternion rotOffset = Quaternion.Inverse(player.transform.rotation) * transform.rotation;

        player.transform.rotation = mainElevator.transform.rotation * rotOffset;
        PlayerController.Instance.ClearCameraMovement();
        player.transform.position = mainElevator.transform.TransformPoint(posOffset);
    }
}
