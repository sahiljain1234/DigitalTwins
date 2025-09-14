using UnityEngine;
using UnityEngine.AI; // For NavMeshAgent

public class RunDummyRun : MonoBehaviour
{
    private NavMeshAgent agent;
    public Material redMaterial; // Assign this in the inspector
    private GameObject[] safeSpaces;
    private bool simulationRunning = false;
    private float timeStandingStill = 0f;
    private Vector3 lastPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameManagerScript.StartSimulation += OnStartSimulation;
    }

    private void OnStartSimulation()
    {
        safeSpaces = GameObject.FindGameObjectsWithTag("SafeSpace");
        simulationRunning = true;
        FindClosestSafeSpace();
    }

    private void Update()
    {
        if (!simulationRunning) return;

        if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
        {
            // If we've reached our destination or can't find a path, stop the simulation.
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                CheckAndMoveToNextSafeSpace();
            }
        }
        // Check if Dummy has moved since last frame
    if (Vector3.Distance(transform.position, lastPosition) > 0.005f) // Adjust the threshold based on your needs
    {
        // Dummy has moved, reset the timer
        timeStandingStill = 0f;
    }
    else
    {
        // Dummy has not moved, increment the timer
        timeStandingStill += Time.deltaTime;
    }

    // Check if Dummy has been standing still for more than 2 seconds
    if (timeStandingStill > 2f)
    {
        StopAndChangeMaterial();
    }

    // Update the last position for the next frame's comparison
    lastPosition = transform.position;
    }


    private void StopAndChangeMaterial()
    {
        // Change material to red
        GetComponent<Renderer>().material = redMaterial;

        // Disable the NavMeshAgent to stop moving
        agent.enabled = false;

        // Optionally, stop checking for movement by disabling simulationRunning or similar logic
        simulationRunning = false;
    }


    private void FindClosestSafeSpace()
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject safeSpace in safeSpaces)
        {
            Vector3 directionToSafeSpace = safeSpace.transform.position - currentPosition;
            float d = directionToSafeSpace.sqrMagnitude;
            if (d < closestDistance)
            {
                closest = safeSpace;
                closestDistance = d;
            }
        }

        if (closest != null)
        {
            agent.SetDestination(closest.transform.position);
        }
    }

    private void CheckAndMoveToNextSafeSpace()
    {
       
        // moving to the next available SafeSpace if possible. If no paths are available, change material.
        if (!PathAvailable())
        {
            if (!FindNewPath())
            {
                // Change material to red and disable the agent to stop moving.
                GetComponent<Renderer>().material = redMaterial;
                //agent.enabled = false;
                simulationRunning = false;
            }
        }
    }

    private bool PathAvailable()
    {
        NavMeshPath path = new NavMeshPath();
        bool pathFound = agent.CalculatePath(agent.destination, path);

        if (!pathFound || path.status != NavMeshPathStatus.PathComplete)
        {
            // If the path couldn't be calculated or isn't complete, then it's not available.
            return false;
        }

        return true; // The path is available and complete.
    }

    private bool FindNewPath()
    {
        NavMeshPath path = new NavMeshPath();
        foreach (GameObject safeSpace in safeSpaces)
        {
            // Avoid recalculating path to the current destination
           // if (safeSpace.transform.position == agent.destination) continue;

            agent.CalculatePath(safeSpace.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                // Set the agent to move towards the new SafeSpace
                agent.SetDestination(safeSpace.transform.position);
                return true; // A new valid path is found
            }
        }
        return false; // No valid path was found to any SafeSpace
    }

    private void OnDestroy()
    {
        GameManagerScript.StartSimulation -= OnStartSimulation;
    }
}
