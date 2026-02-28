using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerVisual : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int Die = Animator.StringToHash(IsDie);
    private const string IsDie = "IsDie";
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }

    private void Update()
    {
        _animator.SetBool(IsRunning, Player.Instance.GetMovementState());
        
        if (!Player.Instance.IsDie)
            AdjustPlayerFacingDirection();
    }

    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        _animator.SetBool(Die, true);
    }

    private void AdjustPlayerFacingDirection()
    {
        var mousePosition = GameInput.Instance.GetMousePosition();
        var playerPosition = Player.Instance.GetPlayerScreenPosition();
        
        _spriteRenderer.flipX = mousePosition.x < playerPosition.x;
    }
}
