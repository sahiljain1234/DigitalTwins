using UnityEngine;

public class CubeFireSpread : MonoBehaviour
{
    public GameObject cubePrefab; // Assign your cube prefab in the inspector
    private float checkDelay = 2f;
    private float cubeSize = 0.2f;
    private bool isSpreading = false; // Initial spreading state set to false

    void OnEnable()
    {
        EventManager.OnStartMove += StartSpreading;
    }

    void OnDisable()
    {
        EventManager.OnStartMove -= StartSpreading;
    }

    void Start()
    {
        // Removed the immediate invocation of SpreadFire
    }

    void StartSpreading()
    {
        // Enable spreading and start SpreadFire with a repeating delay
        isSpreading = true;
        Debug.Log("Start the burn!");
        InvokeRepeating(nameof(SpreadFire), checkDelay, checkDelay); // InvokeRepeating here to start spreading after the event is received
    }

    void SpreadFire()
    {
        if (!isSpreading) return; // Only proceed if spreading has been enabled

        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right, Vector3.up, Vector3.down };

        foreach (var direction in directions)
        {
            RaycastHit hit;
            // Check if there's enough space to duplicate the cube in each direction
            if (!Physics.Raycast(transform.position, direction, out hit, cubeSize))
            {
                Vector3 spawnPosition = transform.position + (direction * cubeSize);
                // Check to ensure the spawn position is not occupied
                if (!Physics.CheckSphere(spawnPosition, cubeSize * 0.5f))
                {
                    Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    // Optional: Stop spreading after reaching a condition (e.g., maximum number of cubes)
    public void StopSpreading()
    {
        isSpreading = false;
        CancelInvoke(nameof(SpreadFire)); // Stop invoking SpreadFire when spreading is stopped
    }
}
