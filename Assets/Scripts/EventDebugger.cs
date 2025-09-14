using UnityEngine;

public class EventDebugger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("🔧 EventDebugger started and ready to monitor events!");
    }
    
    void OnEnable()
    {
        Debug.Log("🔧 EventDebugger enabled - subscribing to events");
        // Subscribe to both event systems
        EventManager.OnStartMove += OnEventManagerStartMove;
        GameManagerScript.StartSimulation += OnGameManagerStartSimulation;
    }
    
    void OnDisable()
    {
        Debug.Log("🔧 EventDebugger disabled - unsubscribing from events");
        // Unsubscribe from both event systems
        EventManager.OnStartMove -= OnEventManagerStartMove;
        GameManagerScript.StartSimulation -= OnGameManagerStartSimulation;
    }
    
    void OnEventManagerStartMove()
    {
        Debug.Log("🔵 EventManager.OnStartMove triggered!");
    }
    
    void OnGameManagerStartSimulation()
    {
        Debug.Log("🟢 GameManagerScript.StartSimulation triggered!");
    }
    
    // Test method to manually trigger events
    [ContextMenu("Test EventManager")]
    public void TestEventManager()
    {
        Debug.Log("🔧 Testing EventManager manually...");
        EventManager.StartMove();
    }
    
    [ContextMenu("Test GameManager")]
    public void TestGameManager()
    {
        Debug.Log("🔧 Testing GameManager manually...");
        GameManagerScript.TriggerStartSimulationStatic();
    }
}
