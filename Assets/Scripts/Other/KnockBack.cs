using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float _knockBackForce = 1.5f;
    [SerializeField] private float _knockBackMovingTimerMax = 0.3f;
    private float _knockBackMovingTimer;

    public bool IsGettingKnockBack { get; private set; } = false;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
            StopKnockBackMovement();
    }

    public void GetKnockBack(Transform damageSource)
    {
        IsGettingKnockBack = true;
        _knockBackMovingTimer = _knockBackMovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce / _rb.mass;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        _rb.linearVelocity = Vector2.zero;
        IsGettingKnockBack = false;
    }
}
