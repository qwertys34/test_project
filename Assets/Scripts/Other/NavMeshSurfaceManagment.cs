using System.Threading.Tasks;
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

    public async Task RebuildNavMeshSurface()
    {
        await _surface.BuildNavMeshAsync();
        //_surface.BuildNavMesh();
    }
}
