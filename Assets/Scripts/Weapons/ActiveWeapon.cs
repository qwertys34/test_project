using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    [SerializeField] private Sword sword;

    public Sword GetActiveWeapon()
    {
        return sword;
    }
    
    private void Update()
    {
        if (!Player.Instance.IsDie)
            FollowMousePosition();
    }
    
    private void FollowMousePosition()
    {
        var mousePosition = GameInput.Instance.GetMousePosition();
        var playerPosition = Player.Instance.GetPlayerScreenPosition();

        transform.rotation = Quaternion.Euler(0f, mousePosition.x < playerPosition.x ? 180f : 0f, 0f);
    }
}
