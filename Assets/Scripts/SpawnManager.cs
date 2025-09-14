using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnLocation; // The location where objects will be spawned
    public GameObject objectPrefab; // The prefab of the object to spawn
    public Slider slider; // Reference to your Slider UI component
    public float spawnRadius = 1.0f; // The radius within which objects will be spawned
    public float minSpeed = 1.5f; // Minimum speed for NavMeshAgent
    public float maxSpeed = 3.5f; // Maximum speed for NavMeshAgent

    public List<GameObject> spawnedObjects = new List<GameObject>();
    private NavMeshSurface navMeshSurface; // Reference to the NavMeshSurface component

    private float previousSliderValue = 0;

    private void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>(); // Find the NavMeshSurface component in the scene

        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface component not found in the scene. Make sure to add it to the floor.");
        }

        // Create the initial object and add it to the list
        SpawnObject();
    }

    private void Update()
    {
        float currentSliderValue = slider.value;

        // Check if the slider value has crossed a whole number (1, 2, 3, etc.)
        if (Mathf.Floor(currentSliderValue) > Mathf.Floor(previousSliderValue))
        {
            SpawnObject();
        }
        else if (Mathf.Floor(currentSliderValue) < Mathf.Floor(previousSliderValue))
        {
            RemoveLastObject();
        }

        previousSliderValue = currentSliderValue;
    }

    private void SpawnObject()
    {
        Vector3 randomSpawnPosition = GetRandomNavMeshPosition(spawnLocation.position, spawnRadius);

        if (randomSpawnPosition != Vector3.zero)
        {
            GameObject newObject = Instantiate(objectPrefab, randomSpawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);

            // Set the NavMeshAgent speed to a random value between minSpeed and maxSpeed
            NavMeshAgent agent = newObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = Random.Range(minSpeed, maxSpeed);
            }
        }
    }

    private void RemoveLastObject()
    {
        if (spawnedObjects.Count > 0)
        {
            GameObject lastObject = spawnedObjects[spawnedObjects.Count - 1];
            spawnedObjects.RemoveAt(spawnedObjects.Count - 1);
            Destroy(lastObject);
        }
    }

    private Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
    {
        NavMeshHit hit;
        Vector3 randomPosition = center + Random.insideUnitSphere * radius;

        if (NavMesh.SamplePosition(randomPosition, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }
}
