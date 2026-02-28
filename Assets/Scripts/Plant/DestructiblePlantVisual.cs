using UnityEngine;

public class DestructiblePlantVisual : MonoBehaviour
{
    [SerializeField] private GameObject prefabPlantDeathVFX;
    private DestructiblePlant _destructiblePlant;

    private void Awake()
    {
        _destructiblePlant = GetComponentInParent<DestructiblePlant>();
    }

    private void Start()
    {
        _destructiblePlant.OnDestructiblePlantTakeDamage += DestructiblePlant_OnDestructiblePlantTakeDamage;
    }

    private void OnDestroy()
    {
        _destructiblePlant.OnDestructiblePlantTakeDamage -= DestructiblePlant_OnDestructiblePlantTakeDamage;
    }

    private void DestructiblePlant_OnDestructiblePlantTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }
    
    private void ShowDeathVFX()
    {
        Instantiate(prefabPlantDeathVFX, transform.position, transform.rotation);
    }
}
