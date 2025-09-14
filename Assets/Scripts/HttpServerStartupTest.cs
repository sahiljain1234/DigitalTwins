using UnityEngine;

public class HttpServerStartupTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("🧪 HttpServerStartupTest: Starting test...");
        
        // Check if UnityHttpServer exists
        UnityHttpServer httpServer = FindObjectOfType<UnityHttpServer>();
        if (httpServer != null)
        {
            Debug.Log("✅ UnityHttpServer found in scene");
            Debug.Log("🧪 Attempting to start server manually...");
            httpServer.StartServer();
        }
        else
        {
            Debug.LogError("❌ UnityHttpServer NOT FOUND in scene!");
            Debug.Log("❌ Please add UnityHttpServer script to a GameObject in your scene");
        }
        
        // Check if ButtonListener exists
        if (ButtonListener.Instance != null)
        {
            Debug.Log("✅ ButtonListener.Instance found");
        }
        else
        {
            Debug.LogError("❌ ButtonListener.Instance is NULL!");
            Debug.Log("❌ Please add ButtonListener script to a GameObject in your scene");
        }
    }
}
