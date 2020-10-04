using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform closedRot;

    [SerializeField]
    private Transform openRot;

    [SerializeField]
    private Transform barrier;

    private bool open = false;
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        if (moving)
		{
            return;
		}

        if (open)
		{
            StartCoroutine(RotateToAngle(barrier, closedRot.eulerAngles, 2f));
            open = false;
        }
        else
		{
            StartCoroutine(RotateToAngle(barrier, openRot.eulerAngles, 2f));
            open = true;
        }
    }

    private IEnumerator RotateToAngle(Transform transform, Vector3 angle, float timeToMove)
    {
        moving = true;

        var currentAng = transform.eulerAngles;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.eulerAngles = Vector3.Lerp(currentAng, angle, t);
            yield return null;
        }

        moving = false;
    }
}
