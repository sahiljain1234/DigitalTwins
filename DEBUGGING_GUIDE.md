# Unity-Android Debugging Guide

## Step-by-Step Debugging Process

### 1. **Setup EventDebugger (IMPORTANT)**
1. In Unity, create an empty GameObject
2. Name it "EventDebugger"
3. Add the `EventDebugger` script to it
4. **You should see these messages in Console when you press Play:**
   ```
   🔧 EventDebugger started and ready to monitor events!
   🔧 EventDebugger enabled - subscribing to events
   ```

### 2. **Test EventDebugger Manually**
1. With EventDebugger selected in Hierarchy
2. In Inspector, right-click on the EventDebugger component
3. Select "Test EventManager" - should see:
   ```
   🔧 Testing EventManager manually...
   🔵 EventManager.OnStartMove triggered!
   ```
4. Select "Test GameManager" - should see:
   ```
   🔧 Testing GameManager manually...
   🟢 GameManagerScript.StartSimulation triggered!
   ```

### 3. **Check Required GameObjects in Scene**
Make sure these exist in your scene:
- **ButtonListener** GameObject with ButtonListener script
- **UnityHttpServer** GameObject with UnityHttpServer script  
- **GameManager** GameObject with GameManagerScript script
- **EventDebugger** GameObject with EventDebugger script

### 4. **Test HTTP Server**
1. Press Play in Unity
2. Look for these messages in Console:
   ```
   Unity HTTP Server started on port 8080
   Listening for requests at: http://localhost:8080/trigger-metal-detection
   ```

### 5. **Test Android App Connection**
1. Run Android app
2. Tap the test button (second button)
3. **In Unity Console, you should see:**
   ```
   🌐 HTTP REQUEST RECEIVED: POST /trigger-metal-detection
   🌐 Request from: [Android IP]
   🌐 User Agent: [Android app info]
   🔥 TRIGGERING METAL DETECTION in Unity digital twin...
   🔥 ButtonListener.Instance found, calling OnButtonClick()...
   🔘 BUTTON CLICKED via ButtonListener!
   🔘 Calling EventManager.StartMove()...
   🔵 EventManager.OnStartMove triggered!
   🔘 Calling GameManagerScript.TriggerStartSimulationStatic()...
   🟢 GameManagerScript.StartSimulation triggered!
   🔘 Simulation started via ButtonListener!
   🔥 Metal detection event triggered successfully!
   ```

## Troubleshooting

### If EventDebugger shows nothing:
- Check if EventDebugger GameObject is active
- Check if EventDebugger script is enabled
- Look for the startup messages in Console

### If no HTTP requests received:
- Check if Unity is in Play mode
- Check if both devices are on same WiFi
- Check Mac firewall settings
- Verify Android app is sending to correct IP

### If ButtonListener.Instance is null:
- Make sure ButtonListener GameObject exists in scene
- Make sure ButtonListener script is attached
- Check if ButtonListener GameObject is active

### If events trigger but simulation doesn't start:
- Check if GameManagerScript exists in scene
- Check if RunDummyRun scripts are attached to dummy objects
- Check if SafeSpace objects are tagged with "SafeSpace"

## Quick Test Commands

### Test from Unity Console:
```csharp
// Test EventManager
EventManager.StartMove();

// Test GameManager
GameManagerScript.TriggerStartSimulationStatic();

// Test ButtonListener
if (ButtonListener.Instance != null)
    ButtonListener.Instance.OnButtonClick();
```

### Test HTTP endpoint manually:
```bash
curl -X POST http://192.168.1.4:8080/trigger-metal-detection \
  -H "Content-Type: application/json" \
  -d '{"message":"test"}'
```
