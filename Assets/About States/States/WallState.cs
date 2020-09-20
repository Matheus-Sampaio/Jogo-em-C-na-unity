using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;
//using System.Numerics;
using UnityEngine;

public class WallState : State
{
    Rigidbody rb;
    float moveSpeed;
    CollisionManager colManager;
    Vector3 avg;
    private float force = 200f;
    public WallState(StateMachine stateMachine, float moveSpeed = 5f) : base(stateMachine)
    {
        this.moveSpeed = moveSpeed;
        rb = stateMachine.GetComponent<Rigidbody>();
        colManager = stateMachine.GetComponent<CollisionManager>();
    }
    public override void OnEnter()
    {
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    public override void OnExit()
    {
        rb.useGravity = true;
        var previousForward=rb.transform.forward;
        previousForward.y = 0;
        rb.transform.up = Vector3.up;
        rb.transform.forward = previousForward;
        rb.freezeRotation = true;
    }
    public override void LogicUpdate()
    {
        rb.transform.forward=Vector3.RotateTowards(rb.transform.forward, colManager.AvgWallDirection(), 0.02f, 0);
    }
    public override void PhysicsUpdate()
    {
        rb.AddForce(colManager.AvgWallDirection() * force);
    }
    public override void Jump()
    {
        stateMachine.SetState(stateMachine.airState);
    }
    public override void Move(Vector2 move)
    {
        var up=move.y * 1f;
        var right = move.x * 1f;
        //throw new System.NotImplementedException();
        rb.MovePosition(rb.transform.position + rb.transform.up * move.y*0.03f + rb.transform.right * move.x*0.03f);
        //
    }
    public override void Grab(bool grab)
    {
        //if(!grab) sta
    }
    public override void OnNotify(object sender, params object[] args)
    {
        if(sender is CollisionManager)
        {
            if(colManager.walls.Count == 0)
            {
                if(colManager.grounds.Count > 0) stateMachine.SetState(stateMachine.walkState);
                else stateMachine.SetState(stateMachine.airState);
            }
        }
    }
}
