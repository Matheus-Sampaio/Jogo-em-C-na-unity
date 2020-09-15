using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State, INotifiable
{
    float moveSpeed;
    float rotSpeed=2f;
    Rigidbody rb;
    Transform t;
    public WalkState(StateMachine stateMachine, float moveSpeed=0.2f): base(stateMachine)
    {
        this.moveSpeed=moveSpeed;
        rb = stateMachine.GetComponent<Rigidbody>();
        t=rb.gameObject.transform;
    }

    public override void Jump()
    {
        stateMachine.SetState(stateMachine.airState);
    }

    public override void Move(Vector2 move)
    {
        rb.MovePosition(t.position+t.forward*move.y*moveSpeed);
        var x=rb.rotation;
        x.eulerAngles+=new Vector3(0,move.x*rotSpeed,0);
        rb.rotation=x;
    }
    public override void OnNotify(object sender, params object[] args)
    {
        if(sender is CollisionManager)
        {
            if((string)args[0]=="Ground" && (bool)args[1]==false && stateMachine.currentState is WalkState)
            {
                stateMachine.SetState(null);
                stateMachine.SetState(stateMachine.airState);
            }
        }
    }
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
}
