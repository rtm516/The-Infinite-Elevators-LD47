using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField]
    private Transform closedPos;

    [SerializeField]
    private Transform openPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator OpenDoor(bool force = false)
	{
        if (force)
		{
            transform.position = openPos.position;
        }
        else
        {
            yield return MoveToPosition(transform, openPos.position, 2f);
        }
    }

    public IEnumerator CloseDoor(bool force = false)
    {
        if (force)
        {
            transform.position = closedPos.position;
        }
        else
        {
            yield return MoveToPosition(transform, closedPos.position, 2f);
        }
    }

    private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
