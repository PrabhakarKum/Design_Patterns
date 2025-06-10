using UnityEngine;

public class JumpCommand : ICommand
{
    private PlayerLocomotionManager _playerLocomotionManager;
    
    public JumpCommand(PlayerLocomotionManager playerLocomotionManager)
    {
        _playerLocomotionManager = playerLocomotionManager;
    }
    public void Execute()
    {
        _playerLocomotionManager.Jump();
    }

    public void Undo()
    {
        
    }
}
