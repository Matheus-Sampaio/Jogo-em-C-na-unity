using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;
//using System.Numerics;
using UnityEngine;

public class WallState : State
{
    Rigidbody rb;
    private float moveSpeed = 0.03f;
    CollisionManager colManager;
    Vector3 avg;
    private float turnSpeed = 3f;
    private float force = 10f;

    private CapsuleCollider col;
    private float previousHeight, previousRadius;
    private Vector3 previousCenter;
    private float climbingHeight = 0.3f, climbingRadius = 0.3f;
    private Vector3 climbingCenter=new Vector3(0,1.3f,0);

    public WallState(StateMachine stateMachine) : base(stateMachine)
    {
        rb = stateMachine.GetComponent<Rigidbody>();
        colManager = stateMachine.GetComponent<CollisionManager>();
        col = stateMachine.GetComponent<CapsuleCollider>();
    }
    public override void OnEnter()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        previousRadius = col.radius;
        previousCenter = col.center;
        previousHeight = col.height;
        col.radius = climbingRadius;
        col.height = climbingHeight;
        col.center = climbingCenter;

        rb.AddForce(colManager.WallDirection()*100, ForceMode.Force);
    }

    public override void OnExit()
    {
        rb.useGravity = true;
        rb.MovePosition(rb.transform.position-(0.5f*rb.transform.forward)); //se afaste da parede antes de consertar seus angulos em relação ao chão
        var previousForward=rb.transform.forward;
        previousForward.y = 0;
        rb.transform.up = Vector3.up; //se alinhe com a normal do chão
        rb.transform.forward = previousForward; //infelizmente a operação anterior pode zuar a direção que o personagem estava olhando antes de sair da parede, isso aqui conserta

        col.radius = previousRadius;
        col.height = previousHeight;
        col.center = previousCenter;
    }
    public override void LogicUpdate()
    {
        avg = colManager.WallDirection();
        //if(rb.velocity.sqrMagnitude>0) rb.transform.forward=Vector3.RotateTowards(rb.transform.forward, avg, turnSpeed*Time.deltaTime, 0);
        //Debug.DrawRay(rb.transform.position, avg, Color.red);
        Debug.DrawRay(rb.transform.position, Vector3.Cross(rb.transform.up, colManager.rightMost), Color.black);
        Debug.DrawRay(rb.transform.position, -Vector3.Cross(rb.transform.up, colManager.leftMost), Color.white);
    }
    public override void PhysicsUpdate()
    {
        //rb.AddForce(avg * force);
    }
    public override void Jump()
    {
        stateMachine.SetState(stateMachine.airState);
    }
    public override void Move(Vector2 move)
    {
        //rb.MovePosition(rb.transform.position + rb.transform.up * up + rb.transform.right * right);
        //rb.AddForce(5*colManager.closest.normalized * force);
        if(move.x>0)
        {
            rb.MovePosition(rb.transform.position + Vector3.Cross(rb.transform.up, colManager.rightMost).normalized * moveSpeed);
            rb.AddForce(5*colManager.rightMost*force);
            rb.transform.forward = Vector3.RotateTowards(rb.transform.forward, colManager.rightMost, turnSpeed * Time.deltaTime, 0);
        }
        else if(move.x<0)
        {
            rb.MovePosition(rb.transform.position + Vector3.Cross(colManager.leftMost, rb.transform.up).normalized * moveSpeed);
            rb.AddForce(5*colManager.leftMost*force);
            rb.transform.forward = Vector3.RotateTowards(rb.transform.forward, colManager.leftMost, turnSpeed * Time.deltaTime, 0);
        }
        if(move.y > 0)
        {
            rb.MovePosition(rb.transform.position + rb.transform.up * moveSpeed);
            //rb.AddForce(5 * colManager.topMost * force);
            rb.transform.forward = Vector3.RotateTowards(rb.transform.forward, colManager.topMost, turnSpeed * Time.deltaTime, 0);
        }
        else if(move.y < 0)
        {
            rb.MovePosition(rb.transform.position + rb.transform.up * -moveSpeed);
            //rb.AddForce(5 * colManager.bottomMost * force);
            rb.transform.forward = Vector3.RotateTowards(rb.transform.forward, colManager.bottomMost, turnSpeed * Time.deltaTime, 0);
        }
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
