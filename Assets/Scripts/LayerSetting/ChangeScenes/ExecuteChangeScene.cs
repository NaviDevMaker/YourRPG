using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteChangeScene:ChangeSceneBase
{  //��b��ɃV�[��J��
    //�ʂŏ�����������Ƃ��ׂ̈ɂ����ăN���X�������N�b�V�����͂���
    [SerializeField]  List<ChangeSceneBase> changeSceneBases;
    public override IEnumerator ChangeScene(ChangeSceneBase changeSceneBase)
    {
       yield return  base.ChangeScene(changeSceneBase);
    }


    




}
