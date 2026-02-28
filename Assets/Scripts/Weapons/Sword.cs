using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    public event EventHandler OnSwordSwing;
    private PolygonCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        AttackColliderTurnOff();
    }

    public void Attack()
    {
        AttackColliderTurnOffOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(_damage);
        }
    }

    public void AttackColliderTurnOff()
    {
        _collider.enabled = false;
    }

    private void AttackColliderTurnOn()
    {
        _collider.enabled = true;
    }

    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
}
