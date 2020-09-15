using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand: Command
{
    private Vector2 movement;
    public MoveCommand(Vector2 v, IMediator actorMediator=null): base(actorMediator)
    {
        this.movement=v;
    }
    public override void Execute() => actorMediator?.Notify(this, movement);
}
