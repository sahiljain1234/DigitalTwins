using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI components

public class ButtonListener : MonoBehaviour
{
    public Button yourButton; // Assign this in the inspector

    void Start()
    {
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        EventManager.StartMove(); // Call the method to trigger the event
    }
}
