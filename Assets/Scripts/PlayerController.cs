using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaDepletionRate = 15f;
    public float staminaRecoveryRate = 10f;

    [Header("Input Settings")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    private CharacterController characterController;
    private Vector3 movement;
    private Vector3 velocity;
    private float currentSpeed;
    private bool isRunning;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("PlayerController script requires a CharacterController component on the GameObject.");
            enabled = false;
            return;
        }

        currentSpeed = walkSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleStamina();
        HandleJump();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            isRunning = Input.GetKey(runKey) && stamina > 0f;
            if (isRunning)
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            Vector3 moveDirection = transform.TransformDirection(movement) * currentSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    private void HandleStamina()
    {
        if (isRunning)
        {
            stamina -= staminaDepletionRate * Time.deltaTime;
            if (stamina <= 0f)
            {
                stamina = 0f;
                isRunning = false; // Prevent running when stamina is 0
            }
        }
        else
        {
            stamina += staminaRecoveryRate * Time.deltaTime;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }
    }
    private void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void OnGUI()
    {
        // Simple stamina bar
        GUI.Box(new Rect(10, 10, 200, 20), "");
        GUI.Box(new Rect(10, 10, stamina * 2, 20), "Stamina");
    }
}
