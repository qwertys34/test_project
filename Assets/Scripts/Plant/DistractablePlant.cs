using System;
using UnityEngine;
public class DestructiblePlant : MonoBehaviour
{
    public event EventHandler OnDestructiblePlantTakeDamage;
    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Sword>())
        {
            Destroy(gameObject);
            OnDestructiblePlantTakeDamage?.Invoke(this, EventArgs.Empty);
            if (NavMeshSurfaceManagment.Instance != null)
            {
                try
                {
                    await NavMeshSurfaceManagment.Instance.RebuildNavMeshSurface();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to rebuild NavMesh: {e.Message}");
                }
            }
        }
    }
}
