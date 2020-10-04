using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portcullis : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform openPos;

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
            StartCoroutine(MoveToPosition(door, openPos.position, 2f));
            UIManager.Instance.setUseVisible(false);
            open = true;
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
