using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Debuff_Attack :ActionMoveBase
{

    [SerializeField] int debuffAmount;
    

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        targetUnit.Battler.AT -= debuffAmount;
        return $"{sourceUnit.Battler.Base.Name}��{Name}�I�I{targetUnit.Battler.Base.Name}�̍U���͂����������I�I";
    }
}
