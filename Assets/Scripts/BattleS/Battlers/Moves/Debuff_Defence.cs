using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Debuff_Defence : ActionMoveBase
{

    [SerializeField] int debuffAmount;

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        targetUnit.Battler.Defence -= debuffAmount;
        return $"{sourceUnit.Battler.Base.Name}の{Name}！！{targetUnit.Battler.Base.Name}の守備力が下がった！！";
    }
}
