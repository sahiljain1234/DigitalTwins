using UnityEngine;
using UnityEngine.UI;

public class StartMovingButton : MonoBehaviour
{
    public Transform newGoal; // Assign the new goal for the MoveTo script in the Inspector

    private void Start()
    {
        Button button = GetComponent<Button>();

        // Add a listener to the button's click event
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        // Trigger the "StartMoving" event with the new goal
        StartMovingEvent.RaiseStartMovingEvent(newGoal);
    }
}