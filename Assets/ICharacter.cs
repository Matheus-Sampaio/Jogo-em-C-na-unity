using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void Jump(bool j);
    void Move(Vector2 v);
    void Grab(bool g);
    void Die();
}
