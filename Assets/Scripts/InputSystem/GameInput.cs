using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private InputSystemActions _inputSystemActions;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerDash;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        _inputSystemActions = new InputSystemActions();
        _inputSystemActions.Enable();
        _inputSystemActions.Player.Attack.started += PlayerAttack_started;
        _inputSystemActions.Player.Dash.started += PlayerDash_started;
    }

    private void PlayerDash_started(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public Vector2 GetMovementVector()
    {
        var inputVector = _inputSystemActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        return mousePosition;
    }

    private void PlayerAttack_started(InputAction.CallbackContext ctx)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public void DisableMovement()
    {
        _inputSystemActions.Player.Move.Disable();
    }
}
