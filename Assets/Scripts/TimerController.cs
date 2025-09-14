using UnityEngine;
using TMPro; // Include this namespace to work with TextMeshPro elements

public class TimerController : MonoBehaviour
{
    public TMP_Text timerText; // Assign your TextMeshPro component in the Inspector
    private float timer;
    private bool timerRunning = false;

    void OnEnable()
    {
        GameManagerScript.StartSimulation += StartTimer; // Subscribe to the StartSimulation event
        GameManagerScript.StopSimulation += StopTimer; // Subscribe to the StopSimulation event
    }

    void OnDisable()
    {
        GameManagerScript.StartSimulation -= StartTimer; // Unsubscribe from the StartSimulation event
        GameManagerScript.StopSimulation -= StopTimer; // Unsubscribe from the StopSimulation event
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void StartTimer()
    {
        timerRunning = true;
        timer = 0; // Optionally reset the timer each time it starts
        UpdateTimerDisplay(); // Ensure timer display is updated immediately
    }

    void StopTimer()
    {
        timerRunning = false;
        // Optionally, you could also reset the timer here if you want it to start from 0 next time it's started
        // timer = 0;
    }

    void UpdateTimerDisplay()
    {
        // Update the TextMeshPro component to show the current time, formatted as you prefer
        timerText.text = timer.ToString("F2"); // Formats the timer to show 2 decimal places
    }
}
