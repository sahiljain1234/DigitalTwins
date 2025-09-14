using System;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpaceDetector : MonoBehaviour
{
    private List<Collider> safeSpaceTriggers;
    private Action onSafeSpaceEntered;

    public void Initialize(List<Collider> safeSpaceTriggers, Action onSafeSpaceEntered)
    {
        this.safeSpaceTriggers = safeSpaceTriggers;
        this.onSafeSpaceEntered = onSafeSpaceEntered;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (safeSpaceTriggers.Contains(other))
        {
            onSafeSpaceEntered?.Invoke();
            Destroy(gameObject); // Destroy the object entering the safe space
        }
    }
}
