using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCommand: Command
{
    private bool grab;
    public GrabCommand(bool grab=true, IMediator actorMediator=null): base(actorMediator)
    {
        this.grab = grab;
    }
    public override void Execute()
    {
        actorMediator?.Notify(this, grab);
    }
}
