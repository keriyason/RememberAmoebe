using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f; //player speed
    [SerializeField] private float jumpHeight = 1.5f; //player jump
    [SerializeField] private float gravity = -20f; //gravity
    [SerializeField] private float turnSpeed = 12f; // how fast the player cam move their head
    [SerializeField] private Transform cameraTarget;// empty child so that camera follows
    [SerializeField] private float mouseSensitivity = 1f; //sens
    [SerializeField] private float minPitch = -30f; // low and high angles that the camera can look at
    [SerializeField] private float maxPitch = 60f;

    private CharacterController controller;
    private float yaw; // left and right camera
    private float pitch; // up and down
    private float verticalVelocity; //up and down w grav

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); 
    }

    private void Start()
    {
        yaw = transform.eulerAngles.y; //camera faces the way as player
        Cursor.lockState = CursorLockMode.Locked; //no cursor
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector2 input = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f); //WASD
        Vector3 moveDir = Quaternion.Euler(0f, yaw, 0f) * new Vector3(input.x, 0f, input.y); // Camera based movemnt

        if (moveDir.sqrMagnitude > 0.001f) //turns the player where the camera goes
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity < 0f) //ground check
            verticalVelocity = -2f;

        if (controller.isGrounded && Input.GetButtonDown("Jump")) //jump check
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime; //gravity

        Vector3 velocity = moveDir * moveSpeed; //walking jumping falling movement
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return; //cusor

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * 2f; //right and left ccamera angles
        pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * mouseSensitivity * 2f, minPitch, maxPitch); //no camera flip
        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f); // roates the camera target 
    }
}