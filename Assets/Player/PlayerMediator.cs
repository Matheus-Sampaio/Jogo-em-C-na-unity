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
        TryGetComponent<Animator>(out animator);
        TryGetComponent<ICharacter>(out player);
        TryGetComponent<StateMachine>(out stateMachine);
        TryGetComponent<CollisionManager>(out collisionManager);
        TryGetComponent<Rigidbody>(out rb);
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
                break;
            default:
                break;
        }
    }
    public void Notify(StateMachine stateMachine, params object[] args) => ProcessStateMachine(stateMachine, args);
    public void Notify(CollisionManager collisionManager, params object[] args)
    {
        stateMachine.OnNotify(collisionManager, args);
        if((string)args[0] == "Ground")
        {
            Debug.Log("Collision With Ground Detected");
            if((bool)args[1])
            {
                Debug.Log("Entering");
                animator.SetTrigger("ToGround");
            }
            else
            {
                Debug.Log("Exiting");
                animator.SetTrigger("ToAir");
            }
        }
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
        player?.Jump(j);
        animator?.SetTrigger("ToAir");
    }
    private void ProcessStateMachine(StateMachine stateMachine, params object[] args)
    {

    }
    private void ProcessPlayerStats(PlayerStats playerStats, params object[] args)
    {

    }
}
