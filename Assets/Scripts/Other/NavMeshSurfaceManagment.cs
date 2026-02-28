using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshSurfaceManagment : MonoBehaviour
{
    private NavMeshSurface _surface;
    public static NavMeshSurfaceManagment Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        _surface = GetComponent<NavMeshSurface>();
        _surface.hideEditorLogs = true;
    }

    public void RebuildNuvMeshSurface()
    {
        _surface.BuildNavMesh();
    }
}
