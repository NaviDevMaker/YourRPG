using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealMove : ActionMoveBase
{
    [SerializeField] int heal;

    public int Heal { get => heal;}


    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        sourceUnit.Battler.HealHP(heal);
        return $"{sourceUnit.Battler.Base.Name}‚Ì{Name}\n{sourceUnit.Battler.Base.Name}‚Í{heal}‚Ì‰ñ•œ";
    }
}
