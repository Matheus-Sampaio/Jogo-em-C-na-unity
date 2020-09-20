using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Essa classe não está fazendo nada de útil, apenas delegando pro state, ela é "preguiçosa" e isso é um code smell
//talvez valha a pena retira-la ou transferir todo o código de PlayerMediator pra cá.
public class Player : MonoBehaviour, ICharacter
{
    private StateMachine stateMachine;
    public CommandManager commandManager;
    void Start()
    {
        stateMachine=GetComponent<StateMachine>();
        commandManager=GetComponent<CommandManager>();
        stateMachine.Init(stateMachine.walkState); //give the default state here
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,transform.forward,Color.blue);
    }
    void FixedUpdate()
    {
    }

    //funcões de ICharacter, que são chamadas por comandos:
    public void Jump(bool j)
    {
        Debug.Log("Player jump");
        stateMachine?.DoJump(j);
    }
    public void Move(Vector2 v)
    {
        stateMachine?.DoMove(v);
    }
    public void Grab(bool g)
    {
        stateMachine?.DoGrab(g);
    }
    public void Die()
    {

    }
}
