using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : State, INotifiable
{
    private int airJumps;
    private int airJumpsLeft;
    private float moveSpeed = 0.1f;
    private float rotSpeed=2f;
    private float initialJumpForce=300;
    private float airJumpForce = 200;
    private Rigidbody rb;
    private Transform t;

    private void ResetJumps() => airJumpsLeft = airJumps;
    private void AirJump()
    {
        if(airJumpsLeft > 0)
        {
            //resetar a velocidade no y é importante pra ter um pulo no ar.
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(0, airJumpForce, 0);
            --airJumpsLeft;
        }
    }
    public AirState(StateMachine stateMachine, int airJumps=3): base(stateMachine)
    {
        this.airJumps = airJumps;
        airJumpsLeft = airJumps;
        rb=stateMachine.GetComponent<Rigidbody>();
        t=rb.gameObject.transform;
    }
    public override void OnEnter() 
    {
        if(stateMachine.previousState is WalkState) rb.AddForce(0, initialJumpForce, 0);
    } 
    public override void OnExit() {}
    //mesmo código que o WalkState... isso viola o DRY, talvez eu encapsule isso mais tarde como MoveBehavior ou sei lá.
    public override void Move(Vector2 move)
    {
        rb.MovePosition(t.position+t.forward*move.y*moveSpeed);
        var x=rb.rotation;
        x.eulerAngles+=new Vector3(0,move.x*rotSpeed,0);
        rb.rotation=x;
    }
    public override void Jump()=>AirJump();
    public override void OnNotify(object sender, params object[] args)
    {
        if(sender is CollisionManager)
        {
            if((string)args[0]=="Ground"  && (bool)args[1]==true && stateMachine.currentState is AirState)
            {
                stateMachine.SetState(stateMachine.walkState);
                ResetJumps();
            }
        }
    }
}
