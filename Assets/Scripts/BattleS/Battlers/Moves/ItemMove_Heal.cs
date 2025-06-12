using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemMove_Heal : ItemMoveBase
{
    [SerializeField] int itemheal;
    //[SerializeField] int prace;

    public int ItemHeal { get => itemheal;}
    //public int Prace { get => prace; }

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        sourceUnit.Battler.HealHP(itemheal);
        return $"{sourceUnit.Battler.Base.Name}‚Í{base.Name}‚ğg‚Á‚½I\nHP‚ª{itemheal}‰ñ•œ‚µ‚½I";
    }
}
