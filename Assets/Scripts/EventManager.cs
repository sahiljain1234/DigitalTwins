using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Define the delegate for the event
    public delegate void StartMoveAction();
    // Define the event based on the delegate
    public static event StartMoveAction OnStartMove;

    // Method to trigger the event
    public static void StartMove()
    {
        if (OnStartMove != null)
            OnStartMove();
    }
}
