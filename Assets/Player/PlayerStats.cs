using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//should this really be a monobehavior?
public class PlayerStats: MonoBehaviour
{
    Player player;
    public int health;
    public float speed;
    public float jumpHeight;
    public int energy;

    public void Update()
    {
        //receive messages from scripts here, like the damages and recharging energy
    }
    public void Damage(int i) 
    {
        //place damage logic here
        if(health<=0) Die();
    }
    private void Die()
    {
        //player.OnDie();
    }
}
