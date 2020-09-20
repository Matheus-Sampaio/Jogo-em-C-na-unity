using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.Collections;

public class CollisionManager : MonoBehaviour
{
    //Relaciona cada parede próxima com seus respectivos vetores em relação ao Player
    public Dictionary<Wall,Vector3> walls { get; private set; } = new Dictionary<Wall, Vector3>();
    public List<GameObject> grounds { get; private set; } = new List<GameObject>();
    private IMediator characterMediator;

    void Start()
    {
        characterMediator=GetComponent<IMediator>();
    }
    // Update is called once per frame
    void Update()
    {
        //ToArray é preciso pois isso cria uma cópia do dict que não pode ser modificado pelo OnTrigger(Enter/Exit).
        //Uma modificação que ocorre no container durante este loop quebra a iteração do loop, e é isso que ToList evita.
        foreach(var w in walls.Keys.ToArray<Wall>())
        {
            walls[w] = w.GetVectorToWall(transform.position);
            Debug.DrawRay(transform.position, walls[w]);
        }
    }
    public Vector3 AvgWallDirection()
    {
        Vector3 avg = Vector3.zero;
        foreach(Vector3 v in walls.Values.ToArray())
        {
            avg += v * (1/(v.magnitude)); //o peso é inversamente proporcional a distancia, ou seja, as paredes mais proximas definem com maior relevancia a direção média da "parede" que o personagem esta subindo
        }
        return avg.normalized;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Wall") walls.Add(collider.gameObject.GetComponent<Wall>(), Vector3.zero);
    }
    private void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Wall") walls.Remove(collider.gameObject.GetComponent<Wall>());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {

            //Debug.Log("Collision with ground");
            grounds.Add(collision.gameObject);
            characterMediator?.Notify(this, "Ground", true);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounds.Remove(collision.gameObject);
            characterMediator?.Notify(this, "Ground", false);
        }
    }
}
