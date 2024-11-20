using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public Transform RPG;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat,
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SwitchCameraStyle(CameraStyle.Basic); // Start in Basic mode
    }

    private void Update()
    {
        // Camera style switching based on right-click
        if (Input.GetKey(KeyCode.Mouse1))
        {
            SwitchCameraStyle(CameraStyle.Combat); // Combat mode when holding right-click
        }
        else
        {
            SwitchCameraStyle(CameraStyle.Basic); // Revert to Basic mode on release
        }

        // Rotate orientation based on player position
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if (currentStyle == CameraStyle.Basic)
        {
            // Free movement based on input direction
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            // Align player with combat look-at direction
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;
            playerObj.forward = Vector3.Slerp(playerObj.forward, dirToCombatLookAt.normalized, Time.deltaTime * rotationSpeed);
        }

        if(currentStyle == CameraStyle.Combat)
        {

        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        if (currentStyle == newStyle) return; // Only switch if the style changes

        // Disable both cameras initially
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        // Enable the correct camera based on mode
        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);

        // Update current camera style
        currentStyle = newStyle;
    }
}
