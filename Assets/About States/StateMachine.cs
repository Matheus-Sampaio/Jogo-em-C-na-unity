using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine: MonoBehaviour, INotifiable
{
    private IMediator characterMediator;
    public State currentState { get; private set; }
    public State previousState { get; private set; } //memoria, importante para saber como o estado atual deve agir dependendo do passado imediato dele, isso ajuda a desacoplar os estados das implementações de outros estados.

    public WalkState walkState { get; private set; }
    public AirState airState { get; private set; }
    public WallState wallState { get; private set; }
    public void Awake()
    {
        walkState = new WalkState(this);
        airState = new AirState(this);
        wallState = new WallState(this);
    }
    public void Start()
    {
        currentState = walkState;
        string s = "This " + this.gameObject.name + "'s State Machine could not find the IMediator Component!";
        if(!TryGetComponent<IMediator>(out characterMediator)) Debug.Log(s);
    }
    public void Init(State state)
    {
        if(state == null) throw new System.Exception("Need to provide a valid State!");
        currentState = state;
        currentState.OnEnter();
    }
    //é perfeitamente valido usar SetState(null)
    public void SetState(State state)
    {
        previousState = currentState;
        currentState = state;
        previousState?.OnExit();
        currentState?.OnEnter();
        characterMediator.Notify(this, currentState);
    }
    public void DoMove(Vector2 v)
    {
        currentState?.Move(v);
    }
    public void DoJump(bool j)
    {
        currentState?.Jump();
    }
    public void OnNotify(object sender, params object[] args)
    {
        walkState.OnNotify(sender, args);
        airState.OnNotify(sender, args);
        wallState.OnNotify(sender, args);
    }
}
