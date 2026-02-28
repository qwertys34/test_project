using System;
using UnityEngine;
public class DestructiblePlant : MonoBehaviour
{
    public event EventHandler OnDestructiblePlantTakeDamage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Sword>())
        {
            Destroy(gameObject);
            OnDestructiblePlantTakeDamage?.Invoke(this, EventArgs.Empty);
            NavMeshSurfaceManagment.Instance.RebuildNuvMeshSurface();
        }
    }
}
