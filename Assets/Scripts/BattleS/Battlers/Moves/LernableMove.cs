using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ǂ̃��x���łǂ̋Z���o���邩��Ή��t����
[System.Serializable]
public class LernableMove
{
    [SerializeField] MoveBase moveBase;//moveBase�̕ϐ����L�����N�^�[��ScriptableObject�̃C���X�y�N�^�[�Őݒ肷�邽��
    [SerializeField] int level;

    public MoveBase MoveBase { get => moveBase;}
    public int Level { get => level;}
}
