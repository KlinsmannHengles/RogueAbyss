// Hello, I'm sorry for thisn, I just created this fuction to use it as intermediate to put an event into the Animation of this Object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    public EnemyBehaviour myFather;

    public void CallChangeIsAttackingToFalse()
    {
        myFather.SetIsAttackingToFalse();
    }

    public void CallDealDamage()
    {
        myFather.DealDamageOnPlayer();
    }
}
