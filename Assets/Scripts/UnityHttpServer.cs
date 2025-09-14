using System;
using System.Collections;
using System.Net;
using System.Text;
using UnityEngine;

public class UnityHttpServer : MonoBehaviour
{
    [Header("Server Configuration")]
    public int port = 8080;
    public string endpoint = "/trigger-metal-detection";
    
    private HttpListener httpListener;
    private bool isRunning = false;
    
    void Start()
    {
        StartServer();
    }
    
    void OnDestroy()
    {
        StopServer();
    }
    
    void OnApplicationQuit()
    {
        StopServer();
    }
    
    public void StartServer()
    {
        if (isRunning) return;
        
        try
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://*:{port}/");
            httpListener.Start();
            isRunning = true;
            
            Debug.Log($"Unity HTTP Server started on port {port}");
            Debug.Log($"Listening for requests at: http://localhost:{port}{endpoint}");
            
            StartCoroutine(HandleRequests());
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to start HTTP server: {e.Message}");
        }
    }
    
    public void StopServer()
    {
        if (!isRunning) return;
        
        try
        {
            httpListener?.Stop();
            httpListener?.Close();
            isRunning = false;
            Debug.Log("Unity HTTP Server stopped");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error stopping HTTP server: {e.Message}");
        }
    }
    
    private IEnumerator HandleRequests()
    {
        while (isRunning && httpListener != null)
        {
            if (httpListener.IsListening)
            {
                var context = httpListener.GetContextAsync();
                yield return new WaitUntil(() => context.IsCompleted);
                
                if (context.IsCompletedSuccessfully)
                {
                    ProcessRequest(context.Result);
                }
            }
            yield return null;
        }
    }
    
    private void ProcessRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;
        
        Debug.Log($"🌐 HTTP REQUEST RECEIVED: {request.HttpMethod} {request.Url.AbsolutePath}");
        Debug.Log($"🌐 Request from: {request.RemoteEndPoint}");
        Debug.Log($"🌐 User Agent: {request.UserAgent}");
        
        // Set CORS headers to allow requests from Android app
        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
        
        if (request.HttpMethod == "OPTIONS")
        {
            // Handle preflight request
            response.StatusCode = 200;
            response.Close();
            return;
        }
        
        if (request.Url.AbsolutePath == endpoint)
        {
            if (request.HttpMethod == "POST")
            {
                // Read the request body
                string requestBody = "";
                if (request.HasEntityBody)
                {
                    var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding);
                    requestBody = reader.ReadToEnd();
                    reader.Close();
                }
                
                Debug.Log($"Received POST request with body: {requestBody}");
                
                // Trigger the metal detection event
                TriggerMetalDetection();
                
                // Send success response
                string responseString = "{\"status\":\"success\",\"message\":\"Metal detection triggered\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                response.StatusCode = 200;
                
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            else
            {
                // Method not allowed
                response.StatusCode = 405;
                response.Close();
            }
        }
        else
        {
            // Not found
            response.StatusCode = 404;
            response.Close();
        }
    }
    
    private void TriggerMetalDetection()
    {
        Debug.Log("🔥 TRIGGERING METAL DETECTION in Unity digital twin...");
        
        // Call ButtonListener.Instance.OnButtonClick() as requested
        if (ButtonListener.Instance != null)
        {
            Debug.Log("🔥 ButtonListener.Instance found, calling OnButtonClick()...");
            ButtonListener.Instance.OnButtonClick();
        }
        else
        {
            Debug.LogError("❌ ButtonListener.Instance is NULL! Make sure ButtonListener is in the scene and has the singleton pattern working.");
        }
        
        // You can also trigger other events or perform additional actions here
        Debug.Log("🔥 Metal detection event triggered successfully!");
    }
    
    void OnGUI()
    {
        if (isRunning)
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"Unity HTTP Server running on port {port}");
            GUI.Label(new Rect(10, 30, 400, 20), $"Endpoint: http://localhost:{port}{endpoint}");
        }
        else
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Unity HTTP Server stopped");
        }
    }
}
