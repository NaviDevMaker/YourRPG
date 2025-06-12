using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BOSSBase : BattlerBase
{
    [SerializeField] int suitableLevel;
    [SerializeField] List<string> bossDialogContent;
    [SerializeField] int bossIndex;//��̃X�e�[�W�Ƀ{�X����̈ȏア��Ƃ��ɐU��C���f�b�N�X

    public int SuitableLevel { get => suitableLevel;}
    public List<string> BossDialogContent { get => bossDialogContent;}
    public int BossIndex { get => bossIndex;}
}
