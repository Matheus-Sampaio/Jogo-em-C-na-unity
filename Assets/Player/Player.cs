using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//essa seria a classe que dá a "liga" pra todas as outras que compõem o jogador, isso seria basicamente
//o padrão "Mediator", mas receio que isso acabe virando um GOD OBJECT aka uma classe monolitica entupida de coisa
public class Player : MonoBehaviour, ICharacter
{
    private InputManager inputManager;
    private StateMachine stateMachine;
    private PlayerStats playerStats;
    private Animator animator;
    private Rigidbody rb;
    public CommandManager commandManager;
    void Start()
    {
        TryGetComponent<Animator>(out animator);
        TryGetComponent<StateMachine>(out stateMachine);
        TryGetComponent<PlayerStats>(out playerStats);
        TryGetComponent<CommandManager>(out commandManager);
        TryGetComponent<Rigidbody>(out rb);
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
        stateMachine.DoJump(j);
        //mais logica de pulo aqui, como setar a animação
    }
    public void Move(Vector2 v)
    {
        stateMachine?.DoMove(v);
    }
    public void Grab(bool g)
    {

    }
    public void Die()
    {

    }
}
