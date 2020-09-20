using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//usar design patterns template method + mediator para criar um mediator massa que pode ser usado pra monstros tmb
public class PlayerMediator : MonoBehaviour, IMediator
{
    Animator animator;
    ICharacter player;
    StateMachine stateMachine;
    CollisionManager collisionManager;
    Rigidbody rb;
    private void Start()
    {
        
        animator=GetComponent<Animator>();
        player=GetComponent<ICharacter>();
        stateMachine=GetComponent<StateMachine>();
        collisionManager=GetComponent<CollisionManager>();
        rb=GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
    }
    public void Notify(Command command, params object[] args)
    {
        switch(command)
        {
            case MoveCommand mc:
                Move((Vector2)args[0]);
                break;
            case JumpCommand jc:
                Jump((bool)args[0]);
                break;
            case GrabCommand gc:
                Grab((bool)args[0]);
                break;
            default:
                break;
        }
    }
    public void Notify(StateMachine stateMachine, params object[] args) => ProcessStateMachine(stateMachine, args);
    public void Notify(CollisionManager collisionManager, params object[] args)
    {
        stateMachine.OnNotify(collisionManager, args);
        /*if((string)args[0] == "Ground")
        {
            Debug.Log("Collision With Ground Detected");
            if((bool)args[1])
            {
                animator.SetTrigger("ToGround");
            }
            else
            {
                Debug.Log("Exiting");
                animator.SetTrigger("ToAir");
            }
        }*/
    }

    public void Notify(PlayerStats playerStats, params object[] args) => ProcessPlayerStats(playerStats, args);
    private void Move(Vector2 arg)
    {
        player?.Move(arg);
        if(stateMachine.currentState is WalkState)
        {
            if(arg.y!=0) animator?.SetBool("IsWalking", true);
            else animator?.SetBool("IsWalking", false);
        }
        
    }
    private void Jump(bool j)
    {
        Debug.Log("PlayerMediator Jump");
        player?.Jump(j);
        //animator?.SetTrigger("ToAir");
    }
    private void Grab(bool g)
    {
        if(g && collisionManager.walls.Count > 0 && !(stateMachine.currentState is WallState)) stateMachine.SetState(stateMachine.wallState);
        if(!g)
        {
            if(stateMachine.currentState is WallState)
            {
                if(collisionManager.grounds.Count > 0) stateMachine.SetState(stateMachine.walkState);
                else stateMachine.SetState(stateMachine.airState);
            }
        }
    }
    private void ProcessStateMachine(StateMachine stateMachine, params object[] args)
    {

    }
    private void ProcessPlayerStats(PlayerStats playerStats, params object[] args)
    {

    }
}
