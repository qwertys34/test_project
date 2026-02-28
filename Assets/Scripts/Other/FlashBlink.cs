using System;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.2f;
    
    private SpriteRenderer _spriteRenderer;
    private float _blinkTimer;
    private Material _defaultMaterial;
    private bool _isBlinking;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;

        _isBlinking = true;

        if (_damagableObject is Player player)
        {
            player.OnFlashBlinking += DamagableObject_OnFlashBlinking;
        }
    }

    private void OnDisable()
    {
        if (_damagableObject is Player player)
        {
            player.OnFlashBlinking -= DamagableObject_OnFlashBlinking;
        }
    }

    private void Update()
    {
        if (_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
        }
    }

    private void DamagableObject_OnFlashBlinking(object sender, EventArgs eventArgs)
    {
        SetBlinkMaterial();
    }
    
    private void SetBlinkMaterial()
    {
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }

    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    public void StopBlinking()
    {
        SetDefaultMaterial();
        _isBlinking = false;
    }
}
