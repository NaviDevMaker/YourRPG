using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDialog : DialogBase
{
    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        return base.TypeDialog(line, auto);
    }
}
