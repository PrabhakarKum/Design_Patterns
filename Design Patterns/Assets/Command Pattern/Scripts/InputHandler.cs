using System;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerManager player;
    private PlayerControls playerControls;
    private Stack<ICommand> commandHistory = new Stack<ICommand>();
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Controls.Undo.performed += ctx => UndoLast();
        playerControls.Controls.Jump.performed += ctx => HandleJump();
        playerControls.Controls.Move.performed += ctx => HandleMove(ctx.ReadValue<Vector2>());
        playerControls.Controls.Move.canceled += ctx => player.Stop();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleMove(Vector2 direction)
    {
        Vector3 moveDir = new Vector3(direction.x, 0, direction.y).normalized;
        ICommand moveCommand = new MoveCommand(player, moveDir);
        moveCommand.Execute();
        commandHistory.Push(moveCommand);
    }

    private void UndoLast()
    {
        if (commandHistory.Count > 0)
        {
            ICommand undoCommand = commandHistory.Pop();
            undoCommand.Undo();
        }
    }

    private void HandleJump()
    {
        ICommand jumpCommand = new JumpCommand(player);
        jumpCommand.Execute();
        commandHistory.Push(jumpCommand);
    }
}
