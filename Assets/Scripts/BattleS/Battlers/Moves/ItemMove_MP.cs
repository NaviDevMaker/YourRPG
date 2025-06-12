using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemMove_MP : ItemMoveBase
{
    [SerializeField] int mpHeal;
    //    [SerializeField] int prace;

    public int MpHeal { get => mpHeal; }
    //    public int Prace { get => prace; }

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        sourceUnit.Battler.HealHP(mpHeal);
        return $"{sourceUnit.Battler.Base.Name}‚Í{base.Name}‚ğg‚Á‚½I\nMP‚ª{mpHeal}‰ñ•œ‚µ‚½I";
    }
}
