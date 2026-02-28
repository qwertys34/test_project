using System;
using UnityEngine;
using UnityEngine.AI;
using Adventure.Utils;

[SelectionBase]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private State currentState;
    public bool IsRunning => _navMeshAgent.velocity != Vector3.zero;
    public event EventHandler OnEnemyAttack;
    
    private NavMeshAgent _navMeshAgent;
    
    // Update facing direction
    private float _checkDirectionTime;
    private readonly float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    /*[Header("Idle")]
    [SerializeField] private float idleTimerMax = 2f;
    private float _idleTimer;*/
    
    [Header("Roaming")]
    [SerializeField] private float roamingDistanceMax = 6f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 6f;
    private float _roamingSpeed;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startPosition;
    
    [Header("Chasing")]
    [SerializeField] private bool isChasingEnemy;
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeedMultiplier = 2f;
    private float _chasingSpeed;
    
    [Header("Attacking")]
    [SerializeField] private bool isAttackingEnemy;
    [SerializeField] private float attackingDistance = 1f;
    [SerializeField] private float attackRate = 2f;
    private float _nextAttackTime;
    
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        currentState = startingState;
        
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed *  chasingSpeedMultiplier;
    }

    private void Update()
    {
        UpdateFacingDirection();
        StateHandler();
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        currentState = State.Death;
    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / chasingSpeedMultiplier;
    }

    private void StateHandler()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer <= 0f)
                {
                    Roaming();
                    _roamingTimer = roamingTimerMax;
                }
                CheckState(); 
                break;
            case State.Chasing:
                Chasing();
                CheckState();
                break;
            case State.Attacking:
                Attacking();
                CheckState();
                break;
            case State.Death:
                break;
        }
    }

    private void CheckState()
    {
        if (Player.Instance.IsDie)
        {
            currentState = State.Roaming;
            _navMeshAgent.speed =  _roamingSpeed;
            return;
        }
        var targetPosition = Player.Instance.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, targetPosition);
        State newState = State.Roaming;
        
        if (isChasingEnemy && distanceToPlayer > attackingDistance 
                           && distanceToPlayer <= chasingDistance)
        {
            newState = State.Chasing;
        }
        
        if (isAttackingEnemy && distanceToPlayer <= attackingDistance)
        {
            newState = State.Attacking;
        }
        
        if (newState != currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.speed = _chasingSpeed;
                _navMeshAgent.ResetPath();
            }
            else if (newState == State.Roaming)
            {
                _navMeshAgent.speed = _roamingSpeed;
                _roamingTimer = 0f;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }
        }
        
        currentState = newState;
    }

    private void UpdateFacingDirection()
    {
        if (Time.time > _checkDirectionTime)
        {
            if (IsRunning)
            {
                AdjustFacingDirection(_lastPosition, transform.position);
            }
            else if (currentState == State.Attacking)
            {
                AdjustFacingDirection(transform.position, Player.Instance.transform.position);
            }
            
            _lastPosition = transform.position;
            _checkDirectionTime = Time.time + _checkDirectionDuration;
        }
    }

    private void Chasing()
    {
        var targetPosition = Player.Instance.transform.position;
        _navMeshAgent.SetDestination(targetPosition);
        Debug.DrawLine(transform.position, targetPosition, Color.red);
    }

    private void Attacking()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            
            _nextAttackTime = Time.time + attackRate;
        }
    }

    private void Roaming()
    {
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startPosition + Utils.GetRandomDirection() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private void AdjustFacingDirection(Vector3 startPos, Vector3 endPos)
    {
        if (startPos.x > endPos.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
