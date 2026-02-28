using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    private static readonly int IsDie = Animator.StringToHash(IS_DIE);
    private static readonly int ChasingSpeedMultiplier = Animator.StringToHash(CHASING_SPEED_MULTIPLIER);
    private static readonly int IsRunning = Animator.StringToHash(IS_RUNNING);
    private static readonly int TakeHit = Animator.StringToHash(TAKE_HIT);
    private static readonly int Attack = Animator.StringToHash(ATTACKING);
    [SerializeField] private PolygonCollider2D polygonCollider2D;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject shadowSkeleton;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    // ПОЛЯ В АНИМАТОРЕ
    private const string IS_RUNNING = "IsRunning";
    private const string ATTACKING = "Attack";
    private const string IS_DIE = "IsDie";
    private const string TAKE_HIT = "TakeHit";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PolygonCollider2DTurnOff();
        enemyAI.OnEnemyAttack += EnemyAI_OnEnemyAttack;
        enemyEntity.OnTakeDamage += EnemyEntity_OnTakeDamage;
        enemyEntity.OnDeath += EnemyEntity_OnDeath;
    }

    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= EnemyAI_OnEnemyAttack;
        enemyEntity.OnTakeDamage -= EnemyEntity_OnTakeDamage;
        enemyEntity.OnDeath -= EnemyEntity_OnDeath;
    }

    private void Update()
    {
        _animator.SetBool(IsRunning, enemyAI.IsRunning);
        _animator.SetFloat(ChasingSpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
    }

    private void EnemyEntity_OnDeath(object  sender, EventArgs e)
    {
        _spriteRenderer.sortingOrder = -1;
        shadowSkeleton.SetActive(false);
        _animator.SetBool(IsDie, true);
        StartCoroutine(AnimatorPauseRoutine());
    }

    private IEnumerator AnimatorPauseRoutine()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _animator.enabled = false;
    }
    
    private void EnemyEntity_OnTakeDamage(object sender, EventArgs e)
    { 
        _animator.SetTrigger(TakeHit);
    }

    private void PolygonCollider2DTurnOn()
    {
        polygonCollider2D.enabled = true;
    }
    
    private void PolygonCollider2DTurnOff()
    {
        polygonCollider2D.enabled = false;
    }
    private void EnemyAI_OnEnemyAttack(object sender, EventArgs e)
    {
        _animator.SetTrigger(Attack);
    }
}
