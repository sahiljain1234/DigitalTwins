using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System;


public class GameManagerScript : MonoBehaviour
{
    //Public variables
    public TMP_Text safetyCounter;
    public GameObject safeSpace1;
    public GameObject safeSpace2;
    public static event Action StartSimulation;
    public static event Action StopSimulation;
    public Button startButton;
    public GameObject CompleteText;

    public Transform spawnLocation; // The location where objects will be spawned
    public GameObject objectPrefab; // The prefab of the object to spawn
    public Slider slider; // Reference to your Slider UI component
    public float spawnRadius = 1.0f; // The radius within which objects will be spawned
    public float minSpeed = 1.5f; // Minimum speed for NavMeshAgent
    public float maxSpeed = 3.5f; // Maximum speed for NavMeshAgent

    //Private variables
    private Collider safeSpace1Collider;
    private Collider safeSpace2Collider;
    public List<Collider> SafeSpaceTriggers = new List<Collider>(); // List to store SafeSpace colliders
    private List<GameObject> spawnedObjects = new List<GameObject>();
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

        // Subscribe the TriggerStartSimulation method to the start button's onClick event
        startButton.onClick.AddListener(TriggerStartSimulation);

        // Find and store SafeSpace colliders
        GameObject[] safeSpaces = GameObject.FindGameObjectsWithTag("SafeSpace");
        foreach (GameObject safeSpace in safeSpaces)
        {
            Collider collider = safeSpace.GetComponent<Collider>();
            if (collider != null)
            {
                SafeSpaceTriggers.Add(collider);
            }
        }


    }

    private void Update()
    {
        float currentSliderValue = slider.value;
        safetyCounter.text = "Based on the simulation, you could potentially have " + spawnedObjects.Count.ToString() + " casualties";

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

    // Implement the TriggerStartSimulation method
    private void TriggerStartSimulation()
    {
        // Trigger the StartSimulation event
        StartSimulation?.Invoke();

        Debug.Log("StartSimulation event triggered");

        // Start the Coroutine to wait for 30 seconds then stop the simulation
        StartCoroutine(WaitAndStopSimulation(30)); // 30 seconds delay
    }

    // Coroutine to wait for a specified amount of time before stopping the simulation
    IEnumerator WaitAndStopSimulation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Trigger the StopSimulation event
        StopSimulation?.Invoke();
        CompleteText.SetActive(true);

        Debug.Log("StopSimulation event triggered");
    }

    private void SpawnObject()
    {
        Vector3 randomSpawnPosition = GetRandomNavMeshPosition(spawnLocation.position, spawnRadius);

        if (randomSpawnPosition != Vector3.zero)
        {
            GameObject newObject = Instantiate(objectPrefab, randomSpawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);

            // Attach a new component to handle trigger detection
            SafeSpaceDetector detector = newObject.AddComponent<SafeSpaceDetector>();
            detector.Initialize(SafeSpaceTriggers, () => {
                spawnedObjects.Remove(newObject);
                safetyCounter.text = "Based on the simulation, you could potentially have " + spawnedObjects.Count.ToString() + " casualties";
            });

            // Set the NavMeshAgent speed to a random value between minSpeed and maxSpeed
            NavMeshAgent agent = newObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
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
        Vector3 randomPosition = center + UnityEngine.Random.insideUnitSphere * radius;

        if (NavMesh.SamplePosition(randomPosition, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }
}
