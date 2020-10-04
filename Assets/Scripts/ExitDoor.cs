using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform openRot;

    [SerializeField]
    private Transform door;

    private bool open = false;

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
        if (!open)
        {
            StartCoroutine(RotateToAngle(door, openRot.rotation, 2f));
            open = true;
        }
    }

    private IEnumerator RotateToAngle(Transform transform, Quaternion angle, float timeToMove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, t);
            yield return null;
        }
    }
}
