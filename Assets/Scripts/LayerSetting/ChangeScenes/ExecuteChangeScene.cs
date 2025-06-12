using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteChangeScene:ChangeSceneBase
{  //一秒後にシーん遷移
    //個別で処理が増えるときの為にあえてクラスをワンクッションはさんだ
    [SerializeField]  List<ChangeSceneBase> changeSceneBases;
    public override IEnumerator ChangeScene(ChangeSceneBase changeSceneBase)
    {
       yield return  base.ChangeScene(changeSceneBase);
    }


    




}
