using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSays : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private Material buttonDefault;

    [SerializeField]
    private Material buttonCorrect;

    [SerializeField]
    private Material buttonIncorrect;

    [SerializeField]
    private Material buttonActive;

    [SerializeField]
    private List<GameObject> buttons;

    private bool active = false;
    private GameObject lastClickedObject;
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
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

        // Initial wait
        yield return new WaitForSeconds(0.5f);

        while (active && !solved)
        {
            // Generate buttons order
            List<GameObject> buttonsOrder = new List<GameObject>(buttons);
            buttonsOrder.Shuffle();

            lastClickedObject = null;

            // Show the player the order
            foreach (GameObject button in buttonsOrder)
			{
                MeshRenderer renderer = button.GetComponentInChildren<MeshRenderer>();
                renderer.material = buttonActive;
                yield return new WaitForSeconds(0.5f);
                renderer.material = buttonDefault;
            }

            while (active)
			{
                // Check if they have clicked a button
                if (buttons.Contains(lastClickedObject))
                {
                    // Is the clicked button the next button
                    if (lastClickedObject == buttonsOrder[0])
                    {
                        // Remove the button
                        buttonsOrder.Remove(lastClickedObject);

                        // Show they are correct
                        MeshRenderer renderer = lastClickedObject.GetComponentInChildren<MeshRenderer>();
                        renderer.material = buttonCorrect;
                        yield return new WaitForSeconds(0.5f);
                        renderer.material = buttonDefault;
                    }
                    else
                    {
                        // Show they are wrong and break for reset
                        MeshRenderer renderer = lastClickedObject.GetComponentInChildren<MeshRenderer>();
                        renderer.material = buttonIncorrect;
                        yield return new WaitForSeconds(0.5f);
                        renderer.material = buttonDefault;

                        yield return new WaitForSeconds(0.5f);

                        break;
                    }

                    // Check if we have clicked all the buttons
                    if (buttonsOrder.Count == 0)
					{
                        solved = true;
                        break;
					}

                    lastClickedObject = null;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        // If we are solved remove the wall
        if (solved)
        {
            Deactivate();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
