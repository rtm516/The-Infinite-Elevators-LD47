using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton Pattern
    public static PlayerController Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    #endregion

    [SerializeField]
    private float speed = 12f;

    [SerializeField]
    private Transform cam;

    [SerializeField]
    private float mouseSmooth = 0.3f;

    [SerializeField]
    private float lookSens = 5f;

    private IInteractable targetInteractable;
    private Rigidbody rb;
    private Vector2 lookDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            UIManager.Instance.setPauseVisible(true);
        }

        if (Input.GetKeyDown(KeyCode.E) && targetInteractable != null)
		{
            targetInteractable.Activate();
        }
    }

    void FixedUpdate()
    {
        Move();
        CheckValid();
    }

    private void LateUpdate()
    {
        LookCamera();
    }

    private void Move()
	{
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 movement = (v * forward + h * right).normalized;
        rb.AddForce(movement * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void LookCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector2 lookDelta = new Vector2();
        lookDelta.x = Mathf.Lerp(lookDelta.x, mouseX, mouseSmooth);
        lookDelta.y = Mathf.Lerp(lookDelta.y, mouseY, mouseSmooth);

        lookDelta *= lookSens;

        lookDir += lookDelta;

        lookDir.y = Mathf.Clamp(lookDir.y, -80, 80);

        cam.localRotation = Quaternion.AngleAxis(-lookDir.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(lookDir.x, Vector3.up);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 2f))
        {
            targetInteractable = hit.transform.GetComponent<IInteractable>();

            UIManager.Instance.setUseVisible(targetInteractable != null);
        }
        else
		{
            UIManager.Instance.setUseVisible(false);
        }
    }

    private void CheckValid()
	{
        if (transform.position.y <= -10)
		{
            transform.position = Vector3.up;
		}
	}

    public void ClearCameraMovement()
	{
        lookDir = Vector2.zero;

    }
}
