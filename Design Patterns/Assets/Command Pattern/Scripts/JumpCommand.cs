using UnityEngine;

public class JumpCommand : ICommand
{
    private PlayerManager playerManager;
    
    public JumpCommand(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }
    public void Execute()
    {
        playerManager.Jump();
    }

    public void Undo()
    {
        
    }
}
