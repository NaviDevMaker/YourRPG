using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    //Ui�̊Ǘ�
    //Battler�̊Ǘ�
   public Battler Battler { get; set; }

    public virtual void Setup(Battler battler)
    {
        Battler = battler;//�����̃o�g���[��ݒ�i�L�����N�^�[�̐ݒ�j
        //UI�̏�����
        //Enemy:�摜�Ɩ��O�̐ݒ�
        //Player:�X�e�[�^�X�̐ݒ�
    }


    public virtual void UpdateUI()
    {

    }

    public virtual void UpdateStatus()
    {

    }

    public virtual void ChangeHPColor()
    {

    }
    
}
