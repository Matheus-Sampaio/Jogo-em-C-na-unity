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
    public WallState(StateMachine stateMachine, float moveSpeed = 5f) : base(stateMachine)
    {
        this.moveSpeed = moveSpeed;
        rb = stateMachine.GetComponent<Rigidbody>();
        colManager = stateMachine.GetComponent<CollisionManager>();
    }
    public override void OnEnter()
    {
        rb.useGravity = false;
    }

    public override void OnExit()
    {
        rb.useGravity = true;
    }
    private Vector3 AvgWallDirection()
    {
        int i = 0;
        Vector3 total = new Vector3();
        foreach(var v in colManager.walls.Values.ToList<Vector3>())
        {
            total += v;
            ++i;
        }
        float f = (float)1 / i;
        return total * f;

    }
    public override void Jump()
    {
        throw new System.NotImplementedException();
    }
    public override void Move(Vector2 move)
    {
        throw new System.NotImplementedException();
    }
}
