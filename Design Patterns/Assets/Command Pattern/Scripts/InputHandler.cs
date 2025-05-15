using System;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerManager player;
    private PlayerControls playerControls;
    private ICommand lastCommand = null;
    
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
        lastCommand = moveCommand;
    }

    private void UndoLast()
    {
        if (lastCommand != null)
        {
            lastCommand.Undo();
            lastCommand = null;  // clear to prevent multiple undo's
        }
    }

    private void HandleJump()
    {
        ICommand jumpCommand = new JumpCommand(player);
        jumpCommand.Execute();
    }
}
