using UnityEngine;

public class MoveCommand: ICommand
{
    private Vector3 direction;
    private PlayerManager playerManager;
    public MoveCommand(PlayerManager _playerManager, Vector3 _direction)
    {
        playerManager = _playerManager;
        direction = _direction;
    }
    
    public void Execute()
    { 
        playerManager.Move(direction);
    }
    
    public void Undo()
    {
        playerManager.ReverseMove();
    }

}
