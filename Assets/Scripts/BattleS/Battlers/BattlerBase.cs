using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] int maxHP;
    [SerializeField] int at;
    [SerializeField] int defence;
    [SerializeField] int magicPoint;
    [SerializeField] int exp;
    [SerializeField] int dropGold;
    [SerializeField] Sprite sprite;
    [SerializeField] List<LernableMove> lernableMoves;//�X�N���v�^�u���I�u�W�F�N�g�Őݒ肷�邽��
    
   
    
    //�C���X�y�N�^�|����Z�b�g���Ă���̂�set�͂���Ȃ�
    public string Name { get => name; set => name = value; }
    public int MaxHP { get => maxHP;}
    public int AT { get => at;}
    public Sprite Sprite { get => sprite;}
    public List<LernableMove> LernableMoves { get => lernableMoves; }
    public int Exp { get => exp;}
    public int Defence { get => defence;}
    public int DropGold { get => dropGold;}
    public int MagicPoint { get => magicPoint;}
}
