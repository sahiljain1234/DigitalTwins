using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AddNavMesh : MonoBehaviour
{
    void Start()
    {
        
        GameObject[] floorObjects = GameObject.FindGameObjectsWithTag("Floor");

        foreach (GameObject floorObject in floorObjects)
        {
            // Check if the object already has a NavMeshSurface component
            if (floorObject.GetComponent<NavMeshSurface>() == null)
            {
                // Add a NavMeshSurface component to the object
                NavMeshSurface navMeshSurface = floorObject.AddComponent<NavMeshSurface>();

                // Optionally, configure the NavMeshSurface settings here
                // For example, you can set the 'Build Settings' as per your requirements:
                // navMeshSurface.collectObjects = CollectObjects.Children;
                // navMeshSurface.layerMask = NavMesh.GetAreaFromName("Walkable");
                // ...

                // Build the NavMesh for this surface
                //navMeshSurface.BuildNavMesh();
            }
        }
    }
}