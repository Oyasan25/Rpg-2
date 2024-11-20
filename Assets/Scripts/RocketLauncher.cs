using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject missilePrefab; // The missile prefab to launch
    public Transform launchPoint; // Launch point of the missile
    public KeyCode fireKey = KeyCode.Mouse0; // Fire key
    public float launchForce = 1000f; // Speed of the missile
    public GameObject placeholderMissile; // The visible missile on the launcher tip
    public Transform RocketRotation;
    private bool isInCombatMode;

    private Camera mainCamera;
    private bool canShoot = true; // Prevents multiple shots at once

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        if (Input.GetKeyDown(fireKey) && canShoot)
        {
            // Disable shooting until cooldown is complete and hide placeholder missile
            canShoot = false;
            if (placeholderMissile != null)
            {
                placeholderMissile.SetActive(false);
            }

            // Invoke the missile launch with a slight delay of 0.1 seconds
            Invoke(nameof(DelayedLaunchMissile), 0.1f);
        }
    }

    void DelayedLaunchMissile()
    {
        // Raycast from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint;

        

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPoint = hit.point; // Aim at the hit point
        }
        else
        {
            targetPoint = ray.GetPoint(1000); // Far point if no hit
        }

        // Calculate direction from launch point to target
        Vector3 launchDirection = (targetPoint - launchPoint.position).normalized;

        // Instantiate the missile at launch point with proper orientation
        GameObject missile = Instantiate(missilePrefab, launchPoint.position, Quaternion.LookRotation(launchDirection));

        // Enable the mesh renderer on the missile
        MeshRenderer meshRenderer = missile.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }

        // Apply force to the missile's Rigidbody
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Reset any initial velocity
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse); // Launch with force
        }

        // Reset the ability to shoot and re-enable the placeholder after a cooldown
        Invoke(nameof(ResetCanShoot), 1f); // Adjust delay as needed
        Invoke(nameof(ShowPlaceholderMissile), 3f); // Re-enable placeholder after 3 seconds
    }

    void ResetCanShoot()
    {
        canShoot = true; // Allow shooting again
    }

    void ShowPlaceholderMissile()
    {
        if (placeholderMissile != null)
        {
            placeholderMissile.SetActive(true); // Show the placeholder missile again
        }
    }
}
