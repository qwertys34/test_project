using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(PolygonCollider2D), typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    private int _currentHealth;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;
    
    private BoxCollider2D _boxCollider;
    private  PolygonCollider2D _polygonCollider;
    private EnemyAI _enemyAI;
    [SerializeField] private EnemyStatsSO enemyStatsSo;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        _currentHealth = enemyStatsSo.health;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform ,enemyStatsSo.damage, _polygonCollider);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth > 0)
            OnTakeDamage?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider.enabled = false;
            _polygonCollider.enabled = false;
            _enemyAI.SetDeathState();
            
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}
