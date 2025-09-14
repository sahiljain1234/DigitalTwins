using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireInstantiation : MonoBehaviour
{
    public GameObject firePrefab; // Reference to the fire GameObject prefab
    private bool simulationRunning = true; // Flag to control the simulation state

    void OnEnable()
    {
        GameManagerScript.StopSimulation += StopSpreading; // Subscribe to the StopSimulation event
    }

    void OnDisable()
    {
        GameManagerScript.StopSimulation -= StopSpreading; // Unsubscribe from the StopSimulation event
    }

    void Update()
    {
        // Check for mouse click and if the simulation is still running
        if (simulationRunning && Input.GetMouseButtonDown(0)) // Left mouse button
        {
            // Check if the click is over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // If true, click is on UI element, so return and do nothing
                return;
            }

            // Raycast from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // Instantiate the firePrefab at the hit point with a slight offset to avoid z-fighting
                Vector3 spawnPosition = hit.point + Vector3.up * 0.01f; // Offset by 0.01 units to avoid z-fighting
                Instantiate(firePrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Fire created at " + spawnPosition);
            }
        }
    }

    // Method to handle the StopSimulation event
    private void StopSpreading()
    {
        simulationRunning = false; // Stop the spreading of fire
        Debug.Log("Stopping the spreading of fire.");
    }
}
