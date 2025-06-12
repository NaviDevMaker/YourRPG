using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuffMove_Attack : ActionMoveBase
{
    [SerializeField] int buffAmount;
  
    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        sourceUnit.Battler.AT += buffAmount;
        return $"{sourceUnit.Battler.Base.Name}の{Name}！！{sourceUnit.Battler.Base.Name}の攻撃力が上がった！！";
    }
}



