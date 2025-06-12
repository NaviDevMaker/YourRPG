using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BOSSBase : BattlerBase
{
    [SerializeField] int suitableLevel;
    [SerializeField] List<string> bossDialogContent;
    [SerializeField] int bossIndex;//一つのステージにボスが二体以上いるときに振るインデックス

    public int SuitableLevel { get => suitableLevel;}
    public List<string> BossDialogContent { get => bossDialogContent;}
    public int BossIndex { get => bossIndex;}
}
