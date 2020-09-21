using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Collider coll;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
    }
    public Vector3 GetVectorToWall(Vector3 v)
    {
        return coll.ClosestPoint(v) - v;
    }
}
