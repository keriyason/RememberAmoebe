using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f; //walk speed
    [SerializeField] private float boostMultiplier = 1.8f; //how much faster you go while boosting
    [SerializeField] private float jumpHeight = 1.5f; //jump height
    [SerializeField] private float gravity = -20f; //downward pull
    [SerializeField] private float turnSpeed = 12f; //how fast the player turns
    [SerializeField] private Transform visual; //child model or sprite that turns
    [SerializeField] private Vector3 visualTilt = Vector3.zero; //X 90 = flat facing up, X -90 = flat facing down
    [SerializeField] private Transform cameraTarget; //child the camera follows
    [SerializeField] private float mouseSensitivity = 1f; //mouse look speed
    [SerializeField] private float minPitch = -30f; //lowest camera angle
    [SerializeField] private float maxPitch = 60f; //highest camera angle

    private CharacterController controller;
    private float yaw; //camera left/right angle
    private float pitch; //camera up/down angle
    private float verticalVelocity; //jump and fall speed

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        yaw = transform.eulerAngles.y; //camera starts facing the player's direction
        Cursor.lockState = CursorLockMode.Locked; //hides and locks the mouse
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //esc frees the mouse
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked) //click locks it again
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector2 input = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f); //WASD
        Vector3 moveDir = Quaternion.Euler(0f, yaw, 0f) * new Vector3(input.x, 0f, input.y); //direction relative to the camera

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir) * Quaternion.Euler(visualTilt); //face the move direction plus the tilt
            Transform turner = visual != null ? visual : transform; //turn the visual, or the capsule if none
            turner.rotation = Quaternion.Slerp(turner.rotation, targetRot, turnSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; //keeps the player on the ground

        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity); //jump

        verticalVelocity += gravity * Time.deltaTime; //gravity

        float speed = moveSpeed;
        if (BoostBar.Instance != null && BoostBar.Instance.IsBursting) speed *= boostMultiplier; //faster while boosting

        Vector3 velocity = moveDir * speed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime); //moves the player
    }

    private void LateUpdate()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return; //only look while the mouse is locked

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * 2f; //mouse left/right
        pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * mouseSensitivity * 2f, minPitch, maxPitch); //mouse up/down, limited
        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f); //camera follows this
    }
}