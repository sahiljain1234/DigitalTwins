using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Unity.AI.Navigation;

[CustomEditor(typeof(AddNavMeshSurfaceToTaggedObjects))]
public class AddNavMeshSurfaceToTaggedObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AddNavMeshSurfaceToTaggedObjects myScript = (AddNavMeshSurfaceToTaggedObjects)target;

        if (GUILayout.Button("Add NavMeshSurfaces to Tagged Objects"))
        {
            myScript.AddNavMeshSurfaces();
        }
    }
}

public class AddNavMeshSurfaceToTaggedObjects : MonoBehaviour
{
    public void AddNavMeshSurfaces()
    {
        // Find all objects with the "Floor" tag
        GameObject[] floorObjects = GameObject.FindGameObjectsWithTag("Floor");

        // Loop through each object and add a NavMeshSurface component
        foreach (GameObject floorObject in floorObjects)
        {
            // Check if the object already has a NavMeshSurface component
            if (floorObject.GetComponent<NavMeshSurface>() == null)
            {
                // Add a NavMeshSurface component to the object
                NavMeshSurface navMeshSurface = floorObject.AddComponent<NavMeshSurface>();
                // You can customize the NavMeshSurface settings here if needed
                // For example:
                // navMeshSurface.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;
                // navMeshSurface.collectObjects = CollectObjects.Children;
                //navMeshSurface.layerMask = NavMesh.AllAreas;

               //exclude layer 3 in the navmesh bake    
                int layerMask = 1 << 8;
                navMeshSurface.layerMask = ~layerMask;
                

                //bake the navmesh
                navMeshSurface.BuildNavMesh();
                Debug.Log("NavMeshSurface added to: " + floorObject.name);
            }
            else
            {
                Debug.LogWarning("NavMeshSurface already exists on: " + floorObject.name);
            }
        }
    }
}
