using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    //Relaciona cada parede próxima com seus respectivos vetores em relação ao Player
    public Dictionary<Wall,Vector3> walls { get; private set; } = new Dictionary<Wall, Vector3>();
    private List<GameObject> grounds=new List<GameObject>();
    private IMediator characterMediator;

    void Start()
    {
        TryGetComponent<IMediator>(out characterMediator);
    }
    // Update is called once per frame
    void Update()
    {
        //ToList é preciso pois isso cria uma cópia do dict que não pode ser modificado pelo OnTrigger(Enter/Exit).
        //Uma modificação que ocorre no container durante este loop quebra a iteração do loop, e é isso que ToList evita.
        foreach(var w in walls.Keys.ToList<Wall>())
        {
            walls[w] = w.GetVectorToWall(transform.position);
            Debug.DrawRay(transform.position, walls[w]);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Wall") walls.Add(collider.gameObject.GetComponent<Wall>(), Vector3.zero);
    }
    private void OnTriggerExit(Collider collider)
    {
        walls.Remove(collider.gameObject.GetComponent<Wall>());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("Collision with ground");
            characterMediator.Notify(this, "Ground", true);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground") characterMediator.Notify(this, "Ground", false);
    }
}
