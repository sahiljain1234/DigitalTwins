using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SafetyManager : MonoBehaviour
{
    public GameObject safeSpace1;
    public GameObject safeSpace2;
    public TMP_Text safeCounter;
    public List<GameObject> peopleInBuilding = new List<GameObject>();

    private Collider safeSpace1Collider;
    private Collider safeSpace2Collider;
    public UnityEvent AllRescued;

    // Reference to the SpawnManager GameObject
    public SpawnManager spawnManager;

    private void Start()
    {
        // Initialize SafeCounter with the number of people in the building
        UpdateSafeCounter();

        // Get the Collider components from the safe spaces
        safeSpace1Collider = safeSpace1.GetComponent<Collider>();
        safeSpace2Collider = safeSpace2.GetComponent<Collider>();

        // Optionally, get the list from the SpawnManager GameObject if needed
        // This would require a reference to the SpawnManager and a suitable method to retrieve the list
        if (spawnManager != null)
        {
            peopleInBuilding = new List<GameObject>(spawnManager.spawnedObjects);
        }

    }

    private void UpdateSafeCounter()
    {
        if (safeCounter != null)
        {
            safeCounter.text = peopleInBuilding.Count.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a person in the building
        if (peopleInBuilding.Contains(other.gameObject))
        {
            // Remove the person from the list and update the counter
            peopleInBuilding.Remove(other.gameObject);
            UpdateSafeCounter();

            // Check if all people have reached a safe space
            if (peopleInBuilding.Count == 0)
            {
                AllRescued?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional: Handle case when people exit the safe space before all are rescued
    }
}
