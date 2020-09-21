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
    public Vector3 leftMost, rightMost, topMost, bottomMost, closest;
    private IMediator characterMediator;
    //Depois de alguns testes, eu cheguei nas seguintes regras para alcançar bon resultados do climb:
    //1-a variavel "player height" tenha o mesmo valor da altura do centro do collider.
    //2-o collider tenha formato esferico (pelo menos quando o estado do player for WallState), eu gosto de 0.4fx0.4f
    //3-a variavel "back"  aponte para algum ponto ATRAS do centro do collider, isso ajuda a remover os malditos vetores nulos de distancia entre o player e as paredes
    //4-O collider Trigger esteja na mesma altura que o centro do collider, e que haja uma intersecção entre os colliders, ou seja, nada de espaço vazio entre eles
    private float playerheight = 1.3f;
    private float back = -0.3f;
    void Start()
    {
        characterMediator=GetComponent<IMediator>();
    }
    // Update is called once per frame
    void Update()
    {
        float leftMostValue = float.PositiveInfinity;
        float rightMostValue = float.NegativeInfinity;
        float topMostValue = float.NegativeInfinity;
        float bottomMostValue = float.PositiveInfinity;
        float closestDistance = float.PositiveInfinity;
        //ToArray é preciso pois isso cria uma cópia do dict que não pode ser modificado pelo OnTrigger(Enter/Exit).
        //Uma modificação que ocorre no container durante este loop quebra a iteração do loop, e é isso que ToList evita.
        foreach(var w in walls.Keys.ToArray<Wall>())
        {
            walls[w] = w.GetVectorToWall(transform.position+transform.up*playerheight+transform.forward*back);
            //Debug.DrawRay(transform.position+transform.up*playerheight+transform.forward*back, walls[w]);
            var x=walls[w] - transform.forward;
            x = transform.worldToLocalMatrix * (transform.position+x);
            if(x.x < leftMostValue)
            {
                leftMostValue = x.x;
                leftMost = walls[w];
            }
            if(x.x > rightMostValue)
            {
                rightMostValue = x.x;
                rightMost = walls[w];
            }
            if(x.y>topMostValue)
            {
                topMostValue = x.y;
                topMost = walls[w];
            }
            if(x.y < bottomMostValue)
            {
                bottomMostValue = x.y;
                bottomMost = walls[w];
            }
            if(x.sqrMagnitude<closestDistance)
            {
                closestDistance = x.sqrMagnitude;
                closest = walls[w];
            }
        }
        Debug.DrawRay(transform.position, leftMost, Color.green);
        Debug.DrawRay(transform.position, rightMost, Color.yellow);
    }
    public Vector3 WallDirection()
    {
        float minDistance = float.PositiveInfinity;
        Vector3 smallest = transform.forward;
        Vector3 wall = Vector3.zero;
        foreach(Vector3 v in walls.Values.ToArray())
        {
            if(v.magnitude<minDistance)
            {
                minDistance = v.magnitude;
                smallest = v;
            }
            wall += v * (1 / Mathf.Pow(v.magnitude, 8));
        }
        //return wall.normalized;
        return smallest.normalized;
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
