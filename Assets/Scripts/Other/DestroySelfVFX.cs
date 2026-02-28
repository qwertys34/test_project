using UnityEngine;

public class DestroySelfVFX : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (ps && !ps.IsAlive())
        {
            DeathSelf();
        }
    }
    private void OnDestroy()
    {
        ps = null;
    }

    private void DeathSelf()
    {
        Destroy(gameObject);
    }

}
