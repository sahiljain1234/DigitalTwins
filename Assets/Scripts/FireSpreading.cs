using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public GameObject firePrefab; // Assign your fire cube prefab in the inspector
    public float spreadInterval = 2f; // Time in seconds between spreads
    public int spreadDistance = 1; // Distance in meters to spread the fire, should match the cube size for a tight fit
    private float timer;
    private bool canSpread = false; // Flag to control spreading

    // Directions to check for spreading, excluding up and down.
    private Vector3[] spreadDirections = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    void OnEnable()
    {
        GameManagerScript.StartSimulation += EnableSpreading; // Subscribe to StartSimulation to enable spreading
        GameManagerScript.StopSimulation += StopSpreading; // Subscribe to StopSimulation to disable spreading
    }

    void OnDisable()
    {
        GameManagerScript.StartSimulation -= EnableSpreading; // Unsubscribe from StartSimulation
        GameManagerScript.StopSimulation -= StopSpreading; // Unsubscribe from StopSimulation
    }

    void Start()
    {
        timer = spreadInterval;
    }

    void Update()
    {
        if (!canSpread) return; // Check if spreading is enabled before proceeding

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            // Attempt to spread fire in all horizontal directions
            foreach (Vector3 direction in spreadDirections)
            {
                SpreadFire(direction);
            }
            timer = spreadInterval;
        }
    }

    void EnableSpreading()
    {
        canSpread = true; // Enable spreading when StartSimulation event is received
    }

    void StopSpreading()
    {
        canSpread = false; // Disable spreading when StopSimulation event is received
    }

    void SpreadFire(Vector3 direction)
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, direction, out hit, spreadDistance))
        {
            Vector3 newPosition = transform.position + (direction * spreadDistance);
            if (canSpread) // Check if spreading is still allowed before creating new fire
            {
                GameObject newFireInstance = Instantiate(firePrefab, newPosition, Quaternion.identity);

                // Ensure the new instance can also spread fire
                FireSpread fireSpreadComponent = newFireInstance.GetComponent<FireSpread>();
                if (fireSpreadComponent == null) // If the component does not exist, add it
                {
                    fireSpreadComponent = newFireInstance.AddComponent<FireSpread>();
                }

                // Optionally, immediately enable spreading for the new instance
                // This would depend on whether you want new instances to spread immediately
                // or only after a new event is received.
                fireSpreadComponent.EnableSpreading();
            }
        }
    }
}
