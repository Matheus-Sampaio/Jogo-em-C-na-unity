using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected IMediator actorMediator;
    public Command(IMediator actorMediator=null)=>this.actorMediator=actorMediator;
    public abstract void Execute();
}
