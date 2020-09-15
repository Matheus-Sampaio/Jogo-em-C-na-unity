using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : Command
{
    private bool jump;
    public JumpCommand(bool jump=true, IMediator actorMediator=null): base(actorMediator)
    {
        this.jump = jump;
    }
    public override void Execute()
    {
        if(jump) actorMediator?.Notify(this, jump);
    }
}
