using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadousiBoss:ISpecialBossHandler
{
    EmergeStair emergeStair;
    BossDialog bossDialog;
    BOSSBase bOSSBase;

    BoxCollider2D stairCollider;

    public void HandleDefeat(int bosslayerIndex, SceneObjectManager sceneObjectManager)
    {
        SetComponent();
        CoroutineRunner.Instance.RunCoroutine(DefeatedDialog(sceneObjectManager,bosslayerIndex));

        
    }

    IEnumerator DefeatedDialog(SceneObjectManager sceneObjectManager,int bosslayerIndex)
    {
        PlayerController.Instance.Constraint = true;

        yield return bossDialog.TypeDialog(bOSSBase.BossDialogContent[0], auto: false);
        sceneObjectManager.GridsBossDontDestroy[bosslayerIndex].gameObject.SetActive(false);
        BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        emergeStair.StairMoveAnim(stairCollider);
    }
    void SetComponent()
    {
        Debug.Log("ゲットコンポ");
        GameObject baseObj;
        var emergeStair = GetInActiveObj.Instence.GetSpecifecObj<EmergeStair>();
     
        if(emergeStair != null) this.emergeStair = emergeStair;

        emergeStair.gameObject.SetActive(true);
        stairCollider = emergeStair.gameObject.GetComponent<BoxCollider2D>();
        stairCollider.enabled = false;
        bossDialog = GameObject.FindObjectOfType<BossDialog>();
        BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        baseObj = bossDialog.gameObject;
        bOSSBase = baseObj.gameObject.GetComponent<EncountBOSS>().BossBattler.BossBase;
    }

    

   
}
