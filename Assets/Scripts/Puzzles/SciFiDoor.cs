using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SciFiDoor : MonoBehaviour, IInteractable
{
    [Header("Materials")]
    [SerializeField]
    private Material buttonDefault;

    [SerializeField]
    private Material buttonCorrect;

    [SerializeField]
    private Material buttonIncorrect;

    [Header("Game objects")]
    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private GameObject buttonCancel;

    [SerializeField]
    private GameObject buttonAccept;

    [Header("Doors")]
    [SerializeField]
    private GameObject leftDoor;

    [SerializeField]
    private GameObject rightDoor;

    [SerializeField]
    private Transform leftDoorOpen;

    [SerializeField]
    private Transform rightDoorOpen;


    [Header("Text")]
    [SerializeField]
    private TextMeshPro stickyNote;

    [SerializeField]
    private TextMeshPro codeScreen;

    [Header("Other")]
    [SerializeField]
    private List<GameObject> buttons;

    private bool active = false;
    private GameObject lastClickedObject;
    private Collider collider;
    private string code;
    private string currentCode;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        code = Random.Range(1000, 10000).ToString();
        stickyNote.SetText("Todays code is\n" + code);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && active)
        {
            Deactivate();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && active)
        {
            RaycastHit hit;
            Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                lastClickedObject = hit.transform.gameObject;
            }
        }
    }

    public void Activate()
    {
        active = true;
        camera.SetActive(true);

        Cursor.lockState = CursorLockMode.None;

        collider.enabled = false;

        PlayerController.Instance.gameObject.SetActive(false);

        UIManager.Instance.setCrosshairVisible(false);
        UIManager.Instance.setUseVisible(false);

        StartCoroutine(DoMinigame());
    }

    public void Deactivate()
    {
        active = false;
        camera.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        collider.enabled = true;

        PlayerController.Instance.gameObject.SetActive(true);

        UIManager.Instance.setCrosshairVisible(true);
    }

    private IEnumerator DoMinigame()
    {
        bool solved = false;

        while (active && !solved)
        {
            lastClickedObject = null;
            currentCode = "";
            codeScreen.SetText(currentCode.PadLeft(4, '-'));

            while (active)
            {
                // Check if they have clicked a button
                if (buttons.Contains(lastClickedObject))
                {
                    if (currentCode.Length < 4)
                    {
                        char numberEntered = buttons.IndexOf(lastClickedObject).ToString()[0];

                        currentCode += numberEntered;
                        codeScreen.SetText(currentCode.PadLeft(4, '-'));
                    }
                }
                else if (lastClickedObject == buttonAccept)
                {
                    // Check if we have finished the code
                    if (code == currentCode)
                    {
                        // Show they are wrong and break for reset
                        foreach (GameObject go in buttons)
                        {
                            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
                            renderer.material = buttonCorrect;
                        }

                        yield return new WaitForSeconds(0.5f);

                        foreach (GameObject go in buttons)
                        {
                            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
                            renderer.material = buttonDefault;
                        }

                        solved = true;
                        break;
                    }
                    else
                    {
                        // Show they are wrong and break for reset
                        foreach (GameObject go in buttons)
                        {
                            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
                            renderer.material = buttonIncorrect;
                        }

                        yield return new WaitForSeconds(0.5f);

                        foreach (GameObject go in buttons)
                        {
                            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
                            renderer.material = buttonDefault;
                        }

                        break;
                    }
                }
                else if (lastClickedObject == buttonCancel)
				{
                    Deactivate();

                    break;
                }

                lastClickedObject = null;

                yield return new WaitForSeconds(0.1f);
            }
        }

        // If we are solved remove the wall
        if (solved)
        {
            Deactivate();

            StartCoroutine(MoveToPosition(leftDoor.transform, leftDoorOpen.position, 2f));
            StartCoroutine(MoveToPosition(rightDoor.transform, rightDoorOpen.position, 2f));

            collider.enabled = false;
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
