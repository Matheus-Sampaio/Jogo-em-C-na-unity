using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State: INotifiable
{
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
    }
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Jump();
    public abstract void Move(Vector2 move);
    public virtual void OnNotify(object sender, params object[] args) { }
}