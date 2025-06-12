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
        return $"{sourceUnit.Battler.Base.Name}��{Name}�I�I{targetUnit.Battler.Base.Name}�̎���͂����������I�I";
    }
}
