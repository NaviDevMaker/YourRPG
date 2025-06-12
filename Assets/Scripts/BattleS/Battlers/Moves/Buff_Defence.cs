using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Buff_Defence : ActionMoveBase
{
    [SerializeField] int buffAmount;

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        sourceUnit.Battler.Defence += buffAmount;
        return $"{sourceUnit.Battler.Base.Name}��{Name}�I�I{sourceUnit.Battler.Base.Name}�̎���͂��オ�����I�I";
    }
}
