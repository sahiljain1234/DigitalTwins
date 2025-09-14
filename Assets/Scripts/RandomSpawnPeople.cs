using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // Import the UI namespace


public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnRadius = 5f;
    public float minSpeed = 1.5f;
    public float maxSpeed = 3.5f;
    public Slider spawnSlider; // Reference to the Slider UI component

    private void Start()
    {
        // Register a callback for the slider's value change event
        spawnSlider.onValueChanged.AddListener(OnSliderValueChanged);
        // Spawn objects initially based on the default slider value
        SpawnObjects(Mathf.RoundToInt(spawnSlider.value));
    }

    // Callback method when the slider value changes
    public void OnSliderValueChanged(float value)
    {
        int numberOfObjectsToSpawn = Mathf.RoundToInt(value); // Convert slider value to an integer
        SpawnObjects(numberOfObjectsToSpawn);
    }

    // Spawn objects based on the provided count
    private void SpawnObjects(int count)
    {
        // Destroy previously spawned objects (if any)
        //DestroySpawnedObjects();

        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, spawnRadius, NavMesh.AllAreas))
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, hit.position, Quaternion.identity);
                NavMeshAgent agent = spawnedObject.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    float randomSpeed = Random.Range(minSpeed, maxSpeed);
                    agent.speed = randomSpeed;
                }
                else
                {
                    Debug.LogWarning("NavMeshAgent component not found on the spawned object.");
                }
            }
        }
    }

    // Destroy all previously spawned objects
   
}