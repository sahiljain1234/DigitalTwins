using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI components

public class ButtonListener : MonoBehaviour
{
    public static ButtonListener Instance { get; private set; }
    
    public Button yourButton; // Assign this in the inspector

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("🔘 BUTTON CLICKED via ButtonListener!");
        Debug.Log("🔘 Calling EventManager.StartMove()...");
        EventManager.StartMove(); // Call the method to trigger the event
        
        Debug.Log("🔘 Calling GameManagerScript.TriggerStartSimulationStatic()...");
        // Also trigger the simulation start event
        GameManagerScript.TriggerStartSimulationStatic();
        Debug.Log("🔘 Simulation started via ButtonListener!");
    }
}
