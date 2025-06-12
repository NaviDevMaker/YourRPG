using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveBase : MoveBase
{

   
    [SerializeField] int magicPoint;
    [SerializeField] ActionType type;

    public int MagicPoint { get => magicPoint;}
    public ActionType Type { get => type;}
}

public enum ActionType
{
    Attack,
    Heal,
    Buff,

}





