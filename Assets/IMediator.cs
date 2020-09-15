using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public interface IMediator
{
    void Notify(Command command, params object[] args);
    void Notify(CollisionManager collisionManager, params object[] args);
    void Notify(StateMachine stateMachine, params object[] args);

    //void Notify();
}
