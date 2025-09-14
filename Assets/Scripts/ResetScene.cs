using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class ResetScene : MonoBehaviour
{
    // This method should be called by the button's onClick event
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get the currently active scene
        SceneManager.LoadScene(currentScene.name); // Reload the scene by name
    }
}
