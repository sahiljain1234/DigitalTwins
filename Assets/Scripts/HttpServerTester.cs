using UnityEngine;
using System.Collections;

public class HttpServerTester : MonoBehaviour
{
    [Header("Testing Configuration")]
    public bool autoTestOnStart = true;
    public float testDelay = 2f;
    
    void Start()
    {
        if (autoTestOnStart)
        {
            StartCoroutine(AutoTest());
        }
    }
    
    IEnumerator AutoTest()
    {
        yield return new WaitForSeconds(testDelay);
        
        Debug.Log("=== HTTP Server Setup Test ===");
        
        // Test 1: Check if ButtonListener.Instance exists
        if (ButtonListener.Instance != null)
        {
            Debug.Log("✅ ButtonListener.Instance is available");
        }
        else
        {
            Debug.LogError("❌ ButtonListener.Instance is null! Make sure ButtonListener is in the scene.");
        }
        
        // Test 2: Check if UnityHttpServer is running
        UnityHttpServer httpServer = FindObjectOfType<UnityHttpServer>();
        if (httpServer != null)
        {
            Debug.Log("✅ UnityHttpServer found in scene");
        }
        else
        {
            Debug.LogError("❌ UnityHttpServer not found in scene!");
        }
        
        // Test 3: Test ButtonListener.OnButtonClick() directly
        if (ButtonListener.Instance != null)
        {
            Debug.Log("Testing ButtonListener.OnButtonClick() directly...");
            ButtonListener.Instance.OnButtonClick();
        }
        
        // Test 4: Check if GameManagerScript exists
        GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
        if (gameManager != null)
        {
            Debug.Log("✅ GameManagerScript found in scene");
        }
        else
        {
            Debug.LogWarning("⚠️ GameManagerScript not found in scene - simulation may not start");
        }
        
        Debug.Log("=== Test Complete ===");
    }
    
    [ContextMenu("Test HTTP Server Setup")]
    public void TestSetup()
    {
        StartCoroutine(AutoTest());
    }
}
