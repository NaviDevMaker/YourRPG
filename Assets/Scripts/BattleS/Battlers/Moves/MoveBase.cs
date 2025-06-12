using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//‹Z‚ÌŠî‘bƒf[ƒ^
//[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    [SerializeField] new string name;
   

    public string Name { get => name;}

    public virtual string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        return " ";
    }
   
}
