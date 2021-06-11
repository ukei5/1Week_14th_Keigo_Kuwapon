using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public int hp;
    public int attack;
    public void Attack(Battler target)
    {
        target.hp -= attack;
    }
}
