using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    private Rigidbody rb;

    private Vector3 moveVector;
    private float hMove;
    private float vMove;

    public float mouseSpeed;
    private float xRotation = 0;

    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Transform headBone;
    public bool grabClothes;
    public float money;
    public float completeOrder;
    public float fallOrder;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();

        Camera playerCamera = null;
        if (mainCamera != null)
            playerCamera = mainCamera.GetComponentInChildren<Camera>(true);

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (playerCamera == null)
        {
            enabled = false;
            return;
        }

        if (mainCamera == null)
            mainCamera = playerCamera.gameObject;
    }

    private void Start()
    {
        mouseSpeed = PlayerPrefs.GetFloat("MouseSensitivity", mouseSpeed * 10);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");

        moveVector = mainCamera.transform.forward * vMove + mainCamera.transform.right * hMove;
        moveVector.y = 0;
        moveVector.Normalize();
        

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveVector * playerSpeed;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        if (cameraPivot != null)
            cameraPivot.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (headBone != null)
            headBone.localRotation = Quaternion.Euler(0f, 0f, xRotation);

        transform.Rotate(Vector3.up * mouseX);
        
    }

    
}
    
