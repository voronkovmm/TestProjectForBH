using UnityEngine;

public class PlayerController
{
    public PlayerMovementController MovementController;
    private PlayerVisibilityController _visibilityController;

    public PlayerController(Player player)
    {
        MovementController = new PlayerMovementController(player);
        _visibilityController = new PlayerVisibilityController(player);
    }
    public void Update()
    {
        MovementController.Update();
        _visibilityController.Update();
    }

    public void LateUpdate() => _visibilityController.LateUpdate();
}
