using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager: MonoBehaviour
{
    private IMediator characterMediator;
    private Command moveCommand = new MoveCommand(Vector2.zero);
    private Command jumpCommand = null;
    private Command grabCommand = null;
    void Start() 
    {
        string s = "This " + this.gameObject.name + "'s Command Manager could not find the IMediator Component!";
        if(!TryGetComponent<IMediator>(out characterMediator)) Debug.Log(s);
    }

    public void SetMoveCommand(Vector2 v)=>moveCommand=new MoveCommand(v, characterMediator);
    public void SetJumpCommand(bool j=true)=>jumpCommand=new JumpCommand(j, characterMediator);

    public void SetGrabCommand(bool g) => grabCommand = new GrabCommand(g, characterMediator);
    private void ExecuteCommands()
    {
        moveCommand?.Execute();
        jumpCommand?.Execute();
        grabCommand?.Execute();
        //resetar comando de jump
        jumpCommand=null;
    }
    void FixedUpdate() => ExecuteCommands();
}
