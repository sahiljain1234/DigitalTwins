using UnityEngine;

public static class StartMovingEvent
{
    // Define a delegate for the event
    public delegate void StartMovingEventHandler(Transform newGoal);

    // Define the event itself using the delegate
    public static event StartMovingEventHandler StartMoving;

    // Create a method to raise the event
    public static void RaiseStartMovingEvent(Transform newGoal)
    {
        if (StartMoving != null)
        {
            StartMoving(newGoal);
        }
    }
}