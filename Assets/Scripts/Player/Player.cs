using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D), typeof(KnockBack))]
public class Player : MonoBehaviour
{
    public static Player Instance;
    
    private Rigidbody2D _rb;
    private KnockBack _knockBack;
    private Camera _camera;

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlinking;
    
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCoolDownTime = 0.25f;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool _canDash = true;
    
    private float _defaultSpeed;
    private float  _currentHealth;
    private Vector2 _direction = Vector2.zero;
    private const float MinSpeed = 0.1f;
    private bool _isRunning;
    private bool _canDamage = true;

    public bool IsDie => _currentHealth == 0;
    
    public bool GetMovementState()
    {
        return _isRunning;
    }
    
    private void Awake()
    {
        Instance = this;
        
        _knockBack = GetComponent<KnockBack>();
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }
    
    private void Start()
    {
        _defaultSpeed = movingSpeed;
        _rb.gravityScale = 0;
        _currentHealth = maxHealth;
        
        if (trailRenderer != null)
            trailRenderer.emitting = false;
        
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
    }

    private void Update()
    {
        _direction = GameInput.Instance.GetMovementVector();
    }
    
    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockBack)
            return;
        
        MovementHandler();
    }
    
    public Vector3 GetPlayerScreenPosition()
    {
        var playerScreenPosition = _camera.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(Transform damageSource, int damage, PolygonCollider2D colliderEnemy)
    {
        if (_canDamage && !IsDie && colliderEnemy.isActiveAndEnabled)
        {
            _canDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            Debug.Log("Your current health: " + _currentHealth);
            _knockBack.GetKnockBack(damageSource);
            
            OnFlashBlinking?.Invoke(this, EventArgs.Empty);
            
            StartCoroutine(DamageRecoveryRoutine());
        }
        
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth != 0) return;
        
        _knockBack.StopKnockBackMovement();
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        GameInput.Instance.DisableMovement();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canDamage = true;
    }
    
    private void MovementHandler()
    {
        _rb.MovePosition(_rb.position + _direction * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(_direction.x) > MinSpeed || Mathf.Abs(_direction.y) > MinSpeed)
            _isRunning = true;
        else 
            _isRunning = false;
    }
    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (!IsDie)
        {
            ActiveWeapon.Instance.GetActiveWeapon().Attack();
        }
    }
    
    private void GameInput_OnPlayerDash(object sender, System.EventArgs e)
    {
        if (_canDash)
            StartCoroutine(DashRoutine());
    }
    
    private IEnumerator DashRoutine()
    {
        _canDash = false;
        movingSpeed *= dashSpeed;
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashTime);
        movingSpeed = _defaultSpeed;
        trailRenderer.emitting = false;
        
        yield return new WaitForSeconds(dashCoolDownTime);
        _canDash = true;
    }

}
