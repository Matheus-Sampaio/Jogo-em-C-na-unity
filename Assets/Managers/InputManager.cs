using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private CommandManager commandManager;
    public void Start()
    {
        TryGetComponent<CommandManager>(out commandManager);
    }
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        commandManager.SetMoveCommand(callbackContext.ReadValue<Vector2>());
    }
    public void OnGrab(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.started) commandManager.SetGrabCommand(true);
        if(callbackContext.canceled) commandManager.SetGrabCommand(false);
    }
    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("INputManager OnJump");
        commandManager.SetJumpCommand(true);
    }
}
