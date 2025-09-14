using UnityEngine;

public class CameraLerpToPosition : MonoBehaviour
{
    public Transform cameraGameLocation; // Assign the CameraGameLocation GameObject's Transform in the Inspector
    private bool startLerp = false;
    private float lerpTime = 3.0f; // Duration of the LERP in seconds. Adjust as needed.
    private float currentLerpTime = 0f;

    void OnEnable()
    {
        GameManagerScript.StartSimulation += BeginLerp; // Subscribe to the StartSimulation event
    }

    void OnDisable()
    {
        GameManagerScript.StartSimulation -= BeginLerp; // Unsubscribe from the StartSimulation event
    }

    void Update()
    {
        if (startLerp)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            // Calculate the LERP fraction
            float perc = currentLerpTime / lerpTime;

            // LERP the position
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraGameLocation.position, perc);

            // LERP the rotation
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraGameLocation.rotation, perc);

            // Optionally, stop the LERP once it is complete
            if (perc >= 1.0f)
            {
                startLerp = false; // Stop LERP
            }
        }
    }

    void BeginLerp()
    {
        startLerp = true;
        currentLerpTime = 0f; // Reset the LERP timer
    }
}
