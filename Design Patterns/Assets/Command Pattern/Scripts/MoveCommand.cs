using UnityEngine;

public class MoveCommand: ICommand
{
    private readonly Vector3 _direction;
    private readonly Vector2 _moveInput;
    private readonly PlayerLocomotionManager _playerLocomotionManager;
    public MoveCommand(PlayerLocomotionManager playerLocomotionManager, Vector3 direction, Vector2 moveInput)
    {
        _playerLocomotionManager = playerLocomotionManager;
        _direction = direction;
        _moveInput = moveInput;
    }
    
    public void Execute()
    { 
        _playerLocomotionManager.Move(_direction, _moveInput);
    }
    
    public void Undo()
    {
        _playerLocomotionManager.ReverseMove();
    }

}
